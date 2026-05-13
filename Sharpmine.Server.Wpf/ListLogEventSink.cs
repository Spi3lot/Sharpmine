using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Windows;

using Serilog.Core;
using Serilog.Events;

namespace Sharpmine.Server.Wpf;

public class ListLogEventSink(
    int? maxGlobalLogs = null,
    int? maxClientLogs = null,
    IFormatProvider? formatProvider = null) : ILogEventSink
{

    private readonly ConcurrentDictionary<Guid, ObservableCollection<LogEntry>> _clientLogs = [];

    public ObservableCollection<LogEntry> GlobalLogs { get; } = [];

    public void Emit(LogEvent logEvent)
    {
        string level = logEvent.Level switch
        {
            LogEventLevel.Verbose => "VRB",
            LogEventLevel.Debug => "DBG",
            LogEventLevel.Information => "INF",
            LogEventLevel.Warning => "WRN",
            LogEventLevel.Error => "ERR",
            LogEventLevel.Fatal => "FTL",
            _ => "UKN"
        };

        string exceptionSuffix = (logEvent.Exception is null) ? string.Empty : $" -> {logEvent.Exception.Message}";
        string message = $"[{logEvent.Timestamp:HH:mm:ss} {level}] {logEvent.RenderMessage(formatProvider)}{exceptionSuffix}";
        var entry = new LogEntry(logEvent.Level, message);

        if (logEvent.Properties.TryGetValue("ClientHandlerId", out var idValue)
            && idValue is ScalarValue { Value: Guid id })
        {
            var clientLogList = GetClientLogs(id);
            AddToLimitedList(clientLogList, entry, maxClientLogs);
        }
        else
        {
            AddToLimitedList(GlobalLogs, entry, maxGlobalLogs);
        }
    }

    public ObservableCollection<LogEntry> GetClientLogs(Guid? id)
    {
        return (id is null)
            ? GlobalLogs
            : _clientLogs.GetOrAdd(id.Value, _ => []);
    }

    private static void AddToLimitedList(ObservableCollection<LogEntry> list, LogEntry entry, int? limit = null)
    {
        var dispatcher = Application.Current?.Dispatcher;

        if (dispatcher is not null)
        {
            dispatcher.InvokeAsync(() =>
            {
                list.Add(entry);
                if (limit.HasValue && list.Count > limit.Value) list.RemoveAt(0);
            });

            return;
        }

        lock (list)
        {
            list.Add(entry);
            if (limit.HasValue && list.Count > limit.Value) list.RemoveAt(0);
        }
    }
}

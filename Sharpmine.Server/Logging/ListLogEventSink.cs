using Serilog.Core;
using Serilog.Events;

namespace Sharpmine.Server.Logging;

public class ListLogEventSink(
    int? maxGlobalLogs = null,
    int? maxClientLogs = null,
    IFormatProvider? formatProvider = null
) : ILogEventSink
{

    public List<string> GlobalLogs { get; } = [];

    public Dictionary<Guid, List<string>> ClientLogs { get; } = new();

    public event Action<string>? GlobalLogAdded;

    public event Action<Guid, string>? ClientLogAdded;

    public void Emit(LogEvent logEvent)
    {
        string message = $"[{logEvent.Timestamp:HH:mm:ss} {logEvent.Level}] {logEvent.RenderMessage(formatProvider)}";

        if (logEvent.Properties.TryGetValue("ClientHandlerId", out var idValue)
            && idValue is ScalarValue { Value: Guid id })
        {
            if (!ClientLogs.TryGetValue(id, out var clientLogList))
            {
                clientLogList = [];
                ClientLogs[id] = clientLogList;
            }

            AddToLimitedList(clientLogList, message, maxClientLogs);
            ClientLogAdded?.Invoke(id, message);
        }
        else
        {
            AddToLimitedList(GlobalLogs, message, maxGlobalLogs);
            GlobalLogAdded?.Invoke(message);
        }
    }

    private static void AddToLimitedList(List<string> list, string message, int? limit = null)
    {
        list.Add(message);

        if (list.Count > limit)
        {
            list.RemoveAt(0);
        }
    }

}
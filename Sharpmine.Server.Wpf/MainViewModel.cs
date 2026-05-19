using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using Sharpmine.Server.Infrastructure;

namespace Sharpmine.Server.Wpf;

public class MainViewModel : INotifyPropertyChanged
{

    private readonly ListLogEventSink _sink;

    // For XAML previewer only
    public MainViewModel()
    {
        _sink = new ListLogEventSink();

        _sink.GlobalLogs.Add(new LogEntry(Serilog.Events.LogEventLevel.Information, "[12:00:00 INF] Started server, listening on port 25565"));
        _sink.GlobalLogs.Add(new LogEntry(Serilog.Events.LogEventLevel.Warning, "[12:00:05 WRN] Player \"Spi3lot\" is moving too fast"));
        _sink.GlobalLogs.Add(new LogEntry(Serilog.Events.LogEventLevel.Error, "[12:00:10 ERR] Failed to parse corrupted packet"));
        ConnectedClients.Add(new ClientSession(Guid.CreateVersion7(), "127.0.0.1") { IsConnected = true });
        ConnectedClients.Add(new ClientSession(Guid.CreateVersion7(), "127.0.0.1") { IsConnected = false });
    }

    public MainViewModel(ServerService serverService, ListLogEventSink sink)
    {
        _sink = sink;

        serverService.ClientConnectionEstablished += (_, handler) =>
        {
            Application.Current.Dispatcher.InvokeAsync(() => ConnectedClients.Add(new ClientSession(handler.Id, handler.Ip)));
        };

        serverService.ClientConnectionTerminated += (_, handler) =>
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var existing = ConnectedClients.FirstOrDefault(c => c.Id == handler.Id);
                existing?.IsConnected = false;
            });
        };
    }

    public ObservableCollection<ClientSession> ConnectedClients { get; } = [];

    public ObservableCollection<LogEntry> ActiveLogs => (ShowGlobalLogs)
        ? _sink.GlobalLogs
        : _sink.GetClientLogs(SelectedClient);

    public bool ShowGlobalLogs
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ActiveLogs));
        }
    } = true;

    public Guid? SelectedClient
    {
        get;
        set
        {
            field = value;
            OnPropertyChanged();
            if (!ShowGlobalLogs) OnPropertyChanged(nameof(ActiveLogs));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}

using Sharpmine.Server.Logging;

namespace Sharpmine.Server.Gui;

public partial class Form1 : Form
{

    private readonly Server _server;

    private readonly ListLogEventSink _sink;

    public Form1(Server server, ListLogEventSink sink)
    {
        server.HandleClientsInBackground();
        _server = server;
        _sink = sink;

        InitializeComponent();
        checkBoxShowGlobal.CheckedChanged += (_, _) => RefreshLogs();

        _server.ClientConnectionEstablished += _ =>
        {
            SafeInvoke(() =>
            {
                listBoxClients.DataSource = null;

                listBoxClients.DataSource = _server.ActiveClientHandlers.Keys
                    .Union(_sink.ClientLogs.Keys)
                    .Order()
                    .ToList();
            });
        };

        _sink.GlobalLogAdded += _ =>
        {
            SafeInvoke(() =>
            {
                if (checkBoxShowGlobal.Checked)
                {
                    RefreshLogs();
                }
            });
        };

        _sink.ClientLogAdded += (id, _) =>
        {
            SafeInvoke(() =>
            {
                if (!checkBoxShowGlobal.Checked && listBoxClients.SelectedItem is Guid selectedId && selectedId == id)
                {
                    RefreshLogs();
                }
            });
        };
    }

    private void ListBoxClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!checkBoxShowGlobal.Checked)
        {
            RefreshLogs();
        }
    }

    private void RefreshLogs()
    {
        listBoxLogs.DataSource = null;

        if (checkBoxShowGlobal.Checked)
        {
            listBoxLogs.DataSource = _sink.GlobalLogs;
        }
        else if (listBoxClients.SelectedItem is Guid id
                 && _sink.ClientLogs.TryGetValue(id, out var clientLogList))
        {
            listBoxLogs.DataSource = clientLogList;
        }

        if (listBoxLogs.Items.Count > 0)
        {
            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
        }
    }

    private void SafeInvoke(Action action)
    {
        if (IsDisposed) return;
        if (InvokeRequired) BeginInvoke(action);
        else action();
    }

}
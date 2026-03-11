using Sharpmine.Server.Protocol;

namespace Sharpmine.Server.Gui;

public partial class Form1 : Form
{

    private readonly Server _server;

    public Form1(Server server)
    {
        _server = server;
        _server.HandleClientsInBackground();
        InitializeComponent();
        
        var connectionStatusChangedAction = (ClientHandler handler) =>
        {
            listBoxClients.DataSource = null;
            listBoxClients.DataSource = _server.ActiveClientHandlers.Keys; // TODO: change to .Values after the correctness of the used .Keys (RemoteEndPoint vs LocalEndPoint) was verified
        };

        _server.ClientConnectionEstablished += connectionStatusChangedAction;
        _server.ClientConnectionTerminated += connectionStatusChangedAction;
    }

    private void ListBoxClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        listBoxLogs.DataSource = (listBoxClients.SelectedValue as ClientHandler)?.Logs;
    }

}
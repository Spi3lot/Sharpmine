namespace Sharpmine.Server.Gui;

public partial class Form1 : Form
{

    public Form1()
    {
        InitializeComponent();
        new Server(25565).HandleClientsInBackground();
    }

}
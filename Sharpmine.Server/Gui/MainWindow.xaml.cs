namespace Sharpmine.Server.Gui;

public partial class MainWindow
{

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

}

namespace Sharpmine.Server.Wpf;

public partial class MainWindow
{

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

}

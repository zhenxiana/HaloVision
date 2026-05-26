using HaloVision.App.ViewModels;

namespace HaloVision.App.Views;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}

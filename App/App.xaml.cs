using Microsoft.UI.Xaml;

namespace App;

public partial class App : Application
{
    private Window? MainWindow;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        MainWindow.Activate();
    }

}

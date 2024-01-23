using Microsoft.UI.Xaml;
using WinUIEx;

namespace App;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private async void WindowEx_Closed(object sender, WindowEventArgs args)
    {
        await App.Hosting!.StopAsync();
    }
}

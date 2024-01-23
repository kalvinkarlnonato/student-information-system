using Microsoft.UI.Xaml;
using WinUIEx;

namespace App;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/AppIcon.ico"));
        Title = "Student Information System";
        SetTitleBar(AppTitleBar);
    }

    private async void WindowEx_Closed(object sender, WindowEventArgs args)
    {
        await App.Hosting!.StopAsync();
    }
}

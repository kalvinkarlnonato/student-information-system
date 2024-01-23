using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using WinUIEx;

namespace App;

public partial class App : Application
{

    public static IHost? Hosting { get; private set; }

    public App()
    {
        this.InitializeComponent();

        Hosting = Host.CreateDefaultBuilder().
            UseContentRoot(AppContext.BaseDirectory).
            ConfigureServices((context, services) => {
                services.AddTransient<MainWindow>();
            }).Build();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await Hosting!.StartAsync();
        var MainWindow = Hosting.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }

}

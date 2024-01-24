using App.Contracts;
using App.Services;
using App.ViewModels;
using App.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using WinUIEx;

namespace App;

public partial class App : Application
{
    public static WindowEx? MainWindow { get; } = new MainWindow();

    public IHost? Hosting { get; }

    public static T GetService<T>()
        where T : class
        {
            if ((App.Current as App)!.Hosting!.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
            }
            return service;
        }

    public App()
    {
        this.InitializeComponent();

        Hosting = Host.CreateDefaultBuilder().
            UseContentRoot(AppContext.BaseDirectory).
            ConfigureServices((context, services) =>
            {
                //Services
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<NotificationService>();

                //ViewModels
                services.AddTransient<HomeViewModel>();
                services.AddTransient<SettingViewModel>();
                services.AddTransient<EnlistViewModel>();
                services.AddTransient<SubjectViewModel>();
                services.AddTransient<StudentViewModel>();
                services.AddTransient<MainViewModel>();

                //Views
                services.AddTransient<HomePage>();
                services.AddTransient<SettingPage>();
                services.AddTransient<EnlistPage>();
                services.AddTransient<SubjectPage>();
                services.AddTransient<StudentPage>();
                services.AddTransient<MainWindow>();
            }).Build();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<INavigationService>().ActivateAsync();
    }

}

using App.Contracts;
using App.Services;
using App.ViewModels;
using App.ViewModels.Dialogs;
using App.Views;
using App.Views.Dialogs;
using Library.Contracts;
using Library.Models;
using Library.Services;
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
                services.AddSingleton<TrappingServices>();
                services.AddSingleton<NotificationService>();

                //Library Services
                services.AddSingleton<IDatabaseService<PersonModel>, PersonDataService>();
                services.AddSingleton<IDatabaseService<StudentModel>, StudentDataService>();

                //ViewModels
                services.AddTransient<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<SettingViewModel>();
                services.AddTransient<EnlistViewModel>();
                services.AddTransient<SubjectViewModel>();
                services.AddTransient<StudentViewModel>();
                services.AddTransient<AddStudentViewModel>();
                
                //Views
                services.AddTransient<MainWindow>();
                services.AddTransient<HomePage>();
                services.AddTransient<SettingPage>();
                services.AddTransient<EnlistPage>();
                services.AddTransient<SubjectPage>();
                services.AddTransient<StudentPage>();
                services.AddTransient<AddStudentView>();
            }).Build();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<INavigationService>().ActivateAsync();
    }

}

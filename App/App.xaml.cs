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
        License.LicenseKey = "IRONSUITE.DUMMY.BRAVO.GAME.GMAIL.COM.13354-509A285276-HHWWA5BPSASNMV-TNWOSYSQDUHK-62EXFNZUCU2P-GNYO6VGZBBOV-JYXEEXS2K5CU-MCCZHYSLVFIO-PI6Y2L-TZBVV5RDYVKLUA-DEPLOYMENT.TRIAL-QDPVDA.TRIAL.EXPIRES.23.FEB.2024";

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
                services.AddSingleton<IDatabaseService<SubjectModel>, SubjectDataService>();
                services.AddSingleton<IDatabaseService<SettingModel>, SettingDataService>();
                services.AddSingleton<IDatabaseService<StudentFeeModel>, StudentFeeDataService>();
                services.AddSingleton<IDatabaseService<StudentSubjectModel>, StudentSubjectDataService>();

                //ViewModels
                services.AddSingleton<EnlistViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<MainViewModel>();
                services.AddTransient<HomeViewModel>();
                services.AddTransient<SettingViewModel>();
                services.AddTransient<SubjectViewModel>();
                services.AddTransient<StudentViewModel>();
                services.AddTransient<AddStudentViewModel>();
                services.AddTransient<ShowEnlistViewModel>();
                
                //Views
                services.AddTransient<MainWindow>();
                services.AddTransient<HomePage>();
                services.AddTransient<SettingPage>();
                services.AddTransient<EnlistPage>();
                services.AddTransient<SubjectPage>();
                services.AddTransient<StudentPage>();
                services.AddTransient<AddStudentView>();
                services.AddTransient<ShowEnlistPage>();
            }).Build();
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<INavigationService>().ActivateAsync();
    }

}

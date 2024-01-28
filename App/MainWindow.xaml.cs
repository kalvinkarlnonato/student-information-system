using App.ViewModels;
using App.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;

namespace App;

public sealed partial class MainWindow : WindowEx
{
    public MainViewModel ViewModel { get; private set; }

    public MainWindow()
    {
        this.InitializeComponent();

        ViewModel = App.GetService<MainViewModel>();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, ViewModel.AppIcon));
        Title = ViewModel.TitlePage;
        SetTitleBar(AppTitleBar);
    }

    private void AppNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            AppContentFrame.Navigate(typeof(SettingPage));
        }
        else
        {
            NavigationViewItem? item = args.SelectedItem as NavigationViewItem;
            if (item!.Tag.ToString() == "HomePage")
            {
                AppContentFrame.Navigate(typeof(HomePage));
            }
            if (item!.Tag.ToString() == "StudentPage")
            {
                AppContentFrame.Navigate(typeof(StudentPage));
            }
            if (item!.Tag.ToString() == "SubjectPage")
            {
                AppContentFrame.Navigate(typeof(SubjectPage));
            }
            if (item!.Tag.ToString() == "EnlistPage")
            {
                AppContentFrame.Navigate(typeof(EnlistPage));
            }
        }
    }

    private void AppNavigation_Loaded(object sender, RoutedEventArgs e)
    {
        AppNavigation.SelectedItem=AppNavigation.MenuItems[0];
        AppContentFrame.Navigate(typeof(HomePage));
    }
}

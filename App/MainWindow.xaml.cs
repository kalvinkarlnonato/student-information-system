using App.ViewModels;
using App.Views;
using Library.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUIEx;

namespace App;

public sealed partial class MainWindow : WindowEx
{
    private MainViewModel ViewModel { get; }

    public Frame MainFrame { get; set; }

    public MainWindow()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<MainViewModel>();
        ExtendsContentIntoTitleBar = true;
        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, ViewModel.AppIcon));
        Title = ViewModel.TitlePage;
        SetTitleBar(AppTitleBar);
        MainFrame = AppContentFrame;
    }

    private async void AppNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if(string.IsNullOrEmpty(UserHelpers.ProccessBy) && AppNavigation.SelectedItem != AppNavigation.MenuItems[0])
        {
            ContentDialog dialog = new ContentDialog() { Title = "Signin", Content = "Please signin your name first.", CloseButtonText = "OK", XamlRoot = sender.XamlRoot };
            await dialog.ShowAsync();
            AppNavigation.SelectedItem = AppNavigation.MenuItems[0];
            return;
        }

        if (args.IsSettingsSelected)
        {
            MainFrame.Navigate(typeof(SettingPage));
        }
        else
        {
            NavigationViewItem? item = args.SelectedItem as NavigationViewItem;
            if (item!.Tag.ToString() == "HomePage")
            {
                MainFrame.Navigate(typeof(HomePage));
            }
            if (item!.Tag.ToString() == "StudentPage")
            {
                MainFrame.Navigate(typeof(StudentPage));
            }
            if (item!.Tag.ToString() == "SubjectPage")
            {
                MainFrame.Navigate(typeof(SubjectPage));
            }
            if (item!.Tag.ToString() == "EnlistPage")
            {
                MainFrame.Navigate(typeof(EnlistPage));
            }
        }
    }

    private void AppNavigation_Loaded(object sender, RoutedEventArgs e)
    {
        AppNavigation.SelectedItem=AppNavigation.MenuItems[0];
        MainFrame.Navigate(typeof(HomePage));
    }
}

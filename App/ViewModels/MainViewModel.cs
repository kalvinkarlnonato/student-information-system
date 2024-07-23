using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;
using Windows.ApplicationModel;

namespace App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private string titlePage;

    [ObservableProperty]
    private string appIcon;

    public MainViewModel()
    {
        TitlePage = $"Student Information System v{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";
        appIcon = "Assets/icon.ico";
    }


}

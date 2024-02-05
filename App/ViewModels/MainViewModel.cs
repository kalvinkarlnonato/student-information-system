using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

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
        TitlePage = $"Student Information System v{Assembly.GetExecutingAssembly().GetName().Version!}";
        appIcon = "Assets/AppIcon.ico";
    }


}

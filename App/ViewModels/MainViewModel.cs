using CommunityToolkit.Mvvm.ComponentModel;

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
        TitlePage = "Student Information System";
        appIcon = "Assets/AppIcon.ico";
    }


}

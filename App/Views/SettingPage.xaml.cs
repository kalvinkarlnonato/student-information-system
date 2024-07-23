using App.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Navigation;

namespace App.Views;

public sealed partial class SettingPage : Page
{
    private SettingViewModel ViewModel { get; }
    public SettingPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<SettingViewModel>();
    }
}

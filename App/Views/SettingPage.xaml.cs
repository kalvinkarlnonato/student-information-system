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

    private void SemCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBox cmb = (ComboBox)sender;
        SemTextblock.Text = ViewModel.Semesters[cmb.SelectedIndex].Sem;
    }

    //protected override void OnNavigatedTo(NavigationEventArgs e)
    //{
    //    EdgeTransitionLocation? Edge;
    //    if (e.NavigationMode == NavigationMode.Back)
    //    {
    //        Edge = EdgeTransitionLocation.Right;
    //    }
    //    base.OnNavigatedTo(e);
    //}
}

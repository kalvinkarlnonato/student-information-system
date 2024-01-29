using App.ViewModels;
using App.ViewModels.Dialogs;
using Microsoft.UI.Xaml.Controls;

namespace App.Views.Dialogs;


public sealed partial class ShowEnlistPage : Page
{
    public ContentDialog? contentDialog { get; set; }

    public EnlistViewModel ViewModel { get; }

    public int CloseStatus { get; set; }

    public ShowEnlistPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<EnlistViewModel>();
        this.DataContext = ViewModel;
    }

    private void Cancel_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        contentDialog!.Hide();
    }

    private void Save_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        CloseStatus = 200;
        contentDialog!.Hide();
    }
}
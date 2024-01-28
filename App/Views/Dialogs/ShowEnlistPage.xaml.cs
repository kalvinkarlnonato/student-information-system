using App.ViewModels.Dialogs;
using Microsoft.UI.Xaml.Controls;

namespace App.Views.Dialogs;


public sealed partial class ShowEnlistPage : Page
{
    public ContentDialog? contentDialog { get; set; }

    public ShowEnlistViewModel ViewModel { get; }

    public ShowEnlistPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<ShowEnlistViewModel>();
        this.DataContext = ViewModel;
    }

    private void Cancel_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        contentDialog!.Hide();
    }
}
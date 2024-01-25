using App.ViewModels.Dialogs;
using Microsoft.UI.Xaml.Controls;

namespace App.Views.Dialogs;


public sealed partial class ShowEnlistPage : Page
{
    public ContentDialog? contentDialog { get; set; }

    public ShowEnlistPage()
    {
        this.InitializeComponent();
        this.DataContext = App.GetService<ShowEnlistViewModel>();
    }

    private void Cancel_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        contentDialog!.Hide();
    }
}
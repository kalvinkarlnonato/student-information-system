using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Views.Dialogs;

public sealed partial class ShowEnlistAddingPage : Page
{
    private EnlistViewModel ViewModel;

    public ShowEnlistAddingPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<EnlistViewModel>();
        this.DataContext = ViewModel;
    }

    private void TextBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        TextBox searchBox = (TextBox)sender;
        var filtered = ViewModel.Subjects.Where(sub => (sub.Code!.Contains(searchBox.Text) || sub.Description!.Contains(searchBox.Text)));
        SubjectLists.ItemsSource = filtered;
    }
}

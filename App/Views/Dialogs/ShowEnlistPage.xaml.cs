using App.ViewModels;
using App.ViewModels.Dialogs;
using Microsoft.UI.Xaml;
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

    private async void Add_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if(SubjectsFrame.Visibility == Visibility.Visible)
        {
            AddingFrame.Navigate(typeof(ShowEnlistAddingPage));
            SubjectsFrame.Visibility = Visibility.Collapsed;
            CurriculumCombo.IsEnabled = false;
            RemoveSubject.IsEnabled = false;
        }
        else
        {
            AddingFrame.Content = null;
            SubjectsFrame.Visibility = Visibility.Visible;
            RemoveSubject.IsEnabled = true;
        }
    }
}
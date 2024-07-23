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
        if (AddingFrame.Content != null)
        {
            AddingFrame.Content = null;
            SubjectsFrame.Visibility = Visibility.Visible;
            CurriculumCombo.IsEnabled = true;
            SectionTextBox.IsEnabled = true;
            RemoveSubject.IsEnabled = true;
            return;
        }
        contentDialog!.Hide();
    }

    private void Save_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.MySection = SectionTextBox.Text;
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
            SectionTextBox.IsEnabled = false;
            RemoveSubject.IsEnabled = false;
        }
        else
        {
            AddingFrame.Content = null;
            SubjectsFrame.Visibility = Visibility.Visible;
            CurriculumCombo.IsEnabled = true;
            SectionTextBox.IsEnabled = true;
            RemoveSubject.IsEnabled = true;
        }
    }
}
using App.ViewModels.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Views.Dialogs;

public sealed partial class AddSubjectView : Page
{
    public ContentDialog? contentDialog { get; set; }

    private AddStudentViewModel ViewModel { get; set; }

    public int StudID { get; set; }

    public AddSubjectView()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<AddStudentViewModel>();
    }
    private void AddMoreButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        contentDialog!.Hide();
    }
}


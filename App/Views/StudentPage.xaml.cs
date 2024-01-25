using App.ViewModels;
using App.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class StudentPage : Page
    {
        public StudentPage()
        {
            this.InitializeComponent();
        }

        private async void Add_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var yourPage = new AddStudentView();
            var yourDialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Content = yourPage
            };
            yourPage.contentDialog = yourDialog;
            await yourDialog.ShowAsync();

            //Get the return result of SearchPage
            var yourPageResult = yourPage.StudID;
        }
    }
}

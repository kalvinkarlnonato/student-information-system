using App.ViewModels;
using App.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class SubjectPage : Page
    {
        private SubjectViewModel ViewModel { get; }

        public SubjectPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<SubjectViewModel>();
        }

        private async void Add_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var yourPage = new AddSubjectView();
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

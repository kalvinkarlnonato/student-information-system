using App.ViewModels;
using App.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

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
            var myPage = new AddSubjectView();
            var myDialog = new ContentDialog()
            {
                XamlRoot = XamlRoot,
                Content = myPage
            };
            myPage.contentDialog = myDialog;
            await myDialog.ShowAsync();

            //Get the return result of SearchPage
            var myPageResult = myPage.StudID;
        }
    }
}

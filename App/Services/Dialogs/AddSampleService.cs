using App.Contracts.Dialogs;
using App.Views;
using App.Views.Dialogs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Services.Dialogs;

public class AddSampleService : IAddSampleService
{
    public async void ShowDialog()
    {
        //WindowEx dialog = App.GetService<AddSampleView>();
        //await dialog.ShowMessageDialogAsync("Content", "Title");

        var myPage = App.GetService<AddSubjectView>();
        var myDialog = new ContentDialog()
        {
            Content = myPage
        };
        myPage.contentDialog = myDialog;
        await myDialog.ShowAsync();

        //Get the return result of SearchPage
        var myPageResult = myPage.StudID;
    }

}

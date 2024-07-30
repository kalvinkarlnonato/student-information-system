using App.ViewModels;
using App.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

namespace App.Views;

public sealed partial class EnlistPage : Page
{
    private EnlistViewModel ViewModel;
    public EnlistPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<EnlistViewModel>();
        this.DataContext = ViewModel;
    }

    private void DataGrid_LoadingRow(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }

    private async void Enlist_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (AllStudentLists.SelectedItems.Count == 0) return;
        ViewModel.isDuplicate = false;
        var exist = ViewModel.Enrolled.Any(x => x.ID == ViewModel.SelectedStudent!.ID);
        if (exist)
        {
            ContentDialog dialog = new ContentDialog() { Title = "Oooops!", Content = "This student already exist.\nDo you want to continue?\nPrevious record will be removed instead.", PrimaryButtonText = "Yes", SecondaryButtonText = "Cancel", XamlRoot = this.XamlRoot };
            ContentDialogResult x = await dialog.ShowAsync();
            if (x == ContentDialogResult.Secondary)
                return;
            ViewModel.isDuplicate = true;
        }

        var myPage = App.GetService<ShowEnlistPage>();
        var myDialog = new ContentDialog()
        {
            XamlRoot = this.XamlRoot,
            Content = myPage
        };
        ViewModel.SelectedCurriculumn = 0;
        ViewModel.LoadStudentSubjects(0);
        myPage.contentDialog = myDialog;
        await myDialog.ShowAsync();
        if (myPage.CloseStatus == 200)
        {
            await ViewModel.Save();
            await ViewModel.LoadEnrolled();
        }
    }

    private void Search_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        TextBox searchBox = (TextBox)sender;
        var filtered = ViewModel.Students.Where(std => ($"{std.SID} {std.Person.LastName}, {std.Person.FirstName}".Contains(searchBox.Text)));
        AllStudentLists.ItemsSource = filtered;
    }

    private void SearchEnrolled_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        TextBox searchBox = (TextBox)sender;
        var filtered = ViewModel.Students.Where(std => ($"{std.SID} {std.Person.LastName}, {std.Person.FirstName}".Contains(searchBox.Text)));
        AllEnrolledLists.ItemsSource = filtered;
    }
}

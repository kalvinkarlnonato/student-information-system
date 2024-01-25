using App.ViewModels;
using App.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

namespace App.Views;

public sealed partial class EnlistPage : Page
{
    public EnlistPage()
    {
        this.InitializeComponent();
        this.DataContext = App.GetService<EnlistViewModel>();
    }

    private void DataGrid_LoadingRow(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEventArgs e)
    {
        e.Row.Header = (e.Row.GetIndex() + 1).ToString();
    }

    private async void Enlist_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var myPage = new ShowEnlistPage();
        var myDialog = new ContentDialog()
        {
            XamlRoot = XamlRoot,
            Content = myPage
        };
        myPage.contentDialog = myDialog;
        await myDialog.ShowAsync();
    }
}

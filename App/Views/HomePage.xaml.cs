using App.ViewModels;
using Library.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices;

namespace App.Views;

public sealed partial class HomePage : Page
{
    private HomeViewModel ViewModel { get; }
    public HomePage()
    { 
        this.InitializeComponent();
        ViewModel = App.GetService<HomeViewModel>();
        this.DataContext = ViewModel;
        if(!string.IsNullOrEmpty(UserHelpers.ProccessBy))
        {
            FullNameTextbox.Text = UserHelpers.ProccessBy;
            FullNameTextbox.IsReadOnly = true;
            SignInButton.IsEnabled = false;
        }
    }

    private async void MessageBox(String title, String content, String closeText)
    {
        ContentDialog dialog = new ContentDialog() { Title = title, Content = content, CloseButtonText = closeText, XamlRoot=XamlRoot};
        await dialog.ShowAsync();
    }

    private void Message_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        MessageBox(ViewModel.Title!, ViewModel.Description!, "OK");
    }

    [DllImport("wininet.dll")]
    private extern static bool InternetGetConnectedState(out int description, int reservedValue);

    public static bool IsInternetAvailable()
    {
        int description;
        return InternetGetConnectedState(out description, 0);
    }
    private void Signin_Click(object sender, RoutedEventArgs e)
    {
        Button button = (Button)sender;
        UserHelpers.ProccessBy = FullNameTextbox.Text;
        button.IsEnabled = false;
        FullNameTextbox.IsReadOnly = true;
    }
}

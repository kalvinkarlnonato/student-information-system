using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class EnlistPage : Page
{
    private EnlistViewModel ViewModel { get; }

    public EnlistPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<EnlistViewModel>();
    }
}

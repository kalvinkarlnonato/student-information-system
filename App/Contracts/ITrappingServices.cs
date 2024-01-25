using Microsoft.UI.Xaml.Input;

namespace App.Contracts
{
    public interface ITrappingServices
    {
        bool StudentIDTrapping(object sender, KeyRoutedEventArgs e);
    }
}
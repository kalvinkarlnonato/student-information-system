using App.Contracts;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace App.Services
{
    public class TrappingServices : ITrappingServices
    {
        public bool StudentIDTrapping(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var txt = (TextBox)sender;
            if (e.Key >= VirtualKey.Number0 && e.Key <= VirtualKey.Number9 || e.Key >= VirtualKey.NumberPad0 && e.Key <= VirtualKey.NumberPad9)
            {
                if (txt.Text.Length < 8) return false;
            }
            return (e.Key != VirtualKey.Back && e.Key != VirtualKey.Left && e.Key != VirtualKey.Right && e.Key != VirtualKey.Tab);
        }
    }
}

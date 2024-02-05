using App.Services;
using App.ViewModels;
using App.ViewModels.Dialogs;
using Library.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Windows.System;
using static System.Net.Mime.MediaTypeNames;

namespace App.Views.Dialogs
{
    public sealed partial class AddStudentView : Page
    {

        public ContentDialog? contentDialog { get; set; }

        public int StudID { get; set; } = 10;

        private TrappingServices TrappingServices { get; set; }

        public AddStudentView()
        {
            this.InitializeComponent();
            TrappingServices = App.GetService<TrappingServices>();
        }

        private void Close_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            contentDialog!.Hide();
        }

        private void studentIdTextbox_PreviewKeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(textBox.SelectionStart > 1)
            {
                if (textBox.Text.Length == 2 && e.Key != VirtualKey.Back)
                {
                    textBox.Text += "-";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                if (textBox.Text.Length > 2 && textBox.Text[2] != '-')
                {
                    
                    textBox.Text = Regex.Replace(textBox.Text, "[^0-9.]", "").Insert(2, "-");
                    textBox.SelectionStart = 2;
                }
            }
            e.Handled = TrappingServices.StudentIDTrapping(sender, e);
        }

        private void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            studID.Focus(FocusState.Programmatic);
            studID.SelectionStart = studID.Text.Length;
        }
    }
}

using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace App.Views
{
    public sealed partial class StudentPage : Page
    {
        private StudentViewModel ViewModel { get; }
        public StudentPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<StudentViewModel>();
        }
    }
}

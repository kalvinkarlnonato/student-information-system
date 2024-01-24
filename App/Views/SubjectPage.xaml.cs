using App.ViewModels;
using Microsoft.UI.Xaml.Controls;

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

    }
}

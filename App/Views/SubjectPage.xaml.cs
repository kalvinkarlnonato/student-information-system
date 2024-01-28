using App.ViewModels;
using App.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;

namespace App.Views
{
    public sealed partial class SubjectPage : Page
    {
        public SubjectViewModel ViewModel { get; private set; }

        public SubjectPage()
        {
            this.InitializeComponent();
            ViewModel = App.GetService<SubjectViewModel>();
            this.DataContext = ViewModel;
        }
    }
}

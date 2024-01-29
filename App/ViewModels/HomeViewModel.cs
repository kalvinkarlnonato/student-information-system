using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;
using Windows.System;

namespace App.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        //[NotifyCanExecuteChangedFor(nameof(ClickCommand))]
        private string? title;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? userFullName;

        public HomeViewModel()
        {
            string greet;
            if (DateTime.Now.Hour <= 12) greet = "Good Morning";
            else if (DateTime.Now.Hour <= 16) greet = "Good Afternoon";
            else if (DateTime.Now.Hour <= 20) greet = "Good Evening";
            else greet = "Good night";
            Title = greet;
            Description = "Welcome to student management system. Please rate my work on my page and i am open for suggestions.";
            UserFullName = string.Empty;
        }

        [RelayCommand(IncludeCancelCommand = true)]
        private async Task Click(CancellationToken token)
        {
            try
            {
                await Task.Delay(5_000, token);
                Title = "Thanks for sharing.";
            }
            catch (OperationCanceledException)
            {
                Title = "You canceled your suggestion.";
            }

        }
    }
}

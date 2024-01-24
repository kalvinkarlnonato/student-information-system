using App.Contracts;

namespace App.Services;

public class NavigationService : INavigationService
{
    public async Task ActivateAsync()
    {
        await InitializeAsync();
        App.MainWindow!.Activate();
        await StartupAsync();
    }

    private static async Task InitializeAsync()
    {
        //TODO: Make initialize something here soon. like etc.
        await Task.CompletedTask;
    }

    private static async Task StartupAsync()
    {
        //TODO: Make something here too.
        await Task.CompletedTask;
    }
}

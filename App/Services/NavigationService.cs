using App.Contracts;
using Library.Helpers;

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
        //License.LicenseKey = "IRONSUITE.DUMMY.BRAVO.GAME.GMAIL.COM.13354-509A285276-HHWWA5BPSASNMV-TNWOSYSQDUHK-62EXFNZUCU2P-GNYO6VGZBBOV-JYXEEXS2K5CU-MCCZHYSLVFIO-PI6Y2L-TZBVV5RDYVKLUA-DEPLOYMENT.TRIAL-QDPVDA.TRIAL.EXPIRES.23.FEB.2024";
        ConnectionStringHelpers.host = "localhost";
        ConnectionStringHelpers.username = "vin";
        ConnectionStringHelpers.password = "Dikoalam";
        ConnectionStringHelpers.database = "csusolana";
        await Task.CompletedTask;
    }

    private static async Task StartupAsync()
    {
        //TODO: Make something here too.
        await Task.CompletedTask;
    }
}

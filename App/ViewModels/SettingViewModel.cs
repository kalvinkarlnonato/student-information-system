using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library;
using Library.Helpers;
using Library.Services;
using Microsoft.Extensions.Hosting;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class SettingViewModel : ObservableObject
{
    [ObservableProperty]
    private string host;

    [ObservableProperty]
    private string key;

    private string username;
    private string password;
    private string database;

    [ObservableProperty]
    private string connectionState;

    [RelayCommand]
    private void SetConnections()
    {
        bool isConnected = true;
        bool isLicense = true;
        ConnectionState = string.Empty;
        if (Host.Length > 0)
        {
            isConnected = ConnectionService.checkDB_Conn(Host, ConnectionStringHelpers.username, ConnectionStringHelpers.password, ConnectionStringHelpers.database);
            ConnectionStringHelpers.host = Host;
        }
        if (Key.Length > 0)
        {
            //isLicense = License.IsValidLicense($"IRONSUITE.{Key}");
            //License.LicenseKey = $"IRONSUITE.{Key}";
        }
        if (Host.Length+Key.Length == 0)
        {
            isConnected = ConnectionService.checkDB_Conn(ConnectionStringHelpers.host, ConnectionStringHelpers.username, ConnectionStringHelpers.password, ConnectionStringHelpers.database);
            //string l = "IRONSUITE.DUMMY.BRAVO.GAME.GMAIL.COM.13354-509A285276-HHWWA5BPSASNMV-TNWOSYSQDUHK-62EXFNZUCU2P-GNYO6VGZBBOV-JYXEEXS2K5CU-MCCZHYSLVFIO-PI6Y2L-TZBVV5RDYVKLUA-DEPLOYMENT.TRIAL-QDPVDA.TRIAL.EXPIRES.23.FEB.2024";
            //isLicense = License.IsValidLicense(l);
        }

        if (!isConnected) ConnectionState += "HOST FAILED";
        if (!isLicense) ConnectionState += "LICENSE FAILED";
        if (isConnected&&isLicense) ConnectionState = "OK";
    }

    public SettingViewModel()
    {
        Host = ConnectionStringHelpers.host;
        Key = "DUMMY.BRAVO.GAME.GMAIL.COM.13354-509A285276-HHWWA5BPSASNMV-TNWOSYSQDUHK-62EXFNZUCU2P-GNYO6VGZBBOV-JYXEEXS2K5CU-MCCZHYSLVFIO-PI6Y2L-TZBVV5RDYVKLUA-DEPLOYMENT.TRIAL-QDPVDA.TRIAL.EXPIRES.23.FEB.2024";
        //SetConnections();
    }
}
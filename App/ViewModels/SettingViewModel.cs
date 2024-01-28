using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library;
using Library.Helpers;
using Library.Services;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class SettingViewModel : ObservableObject
{
    [ObservableProperty]
    private string host;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string database;

    [ObservableProperty]
    private string connectionState;

    [RelayCommand]
    private void SetDatabase()
    {
        if (Host == "kk")
        {
            Host = "localhost";
            Username = "root";
            Password = "";
            Database = "csusolana";
            bool isConn = ConnectionService.checkDB_Conn(Host, Username, Password, Database);
            if (!isConn)
            {
                ConnectionState = "FAILED";
                return;
            }
        }
        bool isConnected = ConnectionService.checkDB_Conn(Host, Username, Password,Database);
        if (!isConnected) { 
            ConnectionState = "FAILED";
            return;
        }
        ConnectionState = "OK";
        ConnectionStringHelpers.host = Host;
        ConnectionStringHelpers.username = Username;
        ConnectionStringHelpers.password = Password;
        ConnectionStringHelpers.database = Database;

    }
    public SettingViewModel()
    {
        ConnectionState = "LISTENING TO THE SERVER ...";
        Host = ConnectionStringHelpers.host;
        Username = ConnectionStringHelpers.username;
        Password = ConnectionStringHelpers.password;
        Database = ConnectionStringHelpers.database;
    }
}
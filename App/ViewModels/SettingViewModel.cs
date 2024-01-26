using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library;
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
        Configs.ConnectionString.host = Host;
        Configs.ConnectionString.username = Username;
        Configs.ConnectionString.password = Password;
        Configs.ConnectionString.database = Database;

    }
    public SettingViewModel()
    {
        ConnectionState = "LISTENING TO THE SERVER ...";
        Host = Configs.ConnectionString.host;
        Username = Configs.ConnectionString.username;
        Password = Configs.ConnectionString.password;
        Database = Configs.ConnectionString.database;
    }
}
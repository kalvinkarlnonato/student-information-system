using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class SettingViewModel : ObservableObject
{
    public ObservableCollection<SettingModel> Semesters { get; } = new ObservableCollection<SettingModel>();
    public SettingViewModel()
    {
        Semesters.Add(new SettingModel() { ID = 1, Sem = "2021-2022" });
        Semesters.Add(new SettingModel() { ID = 2, Sem = "2022-2023" });
        Semesters.Add(new SettingModel() { ID = 3, Sem = "2023-2024" });
        Semesters.Add(new SettingModel() { ID = 4, Sem = "2024-2025" });
    }
}
public class SettingModel
{
    public int ID { get; set; }
    public String? Sem { get; set; }
}
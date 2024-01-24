using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public class EnlistViewModel : ObservableObject
{
    public ObservableCollection<EnlistModel> Students { get; } = new ObservableCollection<EnlistModel>();
    public EnlistViewModel()
    {
        Students.Add(new EnlistModel() { ID = 1, Name = "CICS" });
        Students.Add(new EnlistModel() { ID = 2, Name = "CRIM" });
        Students.Add(new EnlistModel() { ID = 3, Name = "AGRI" });
        Students.Add(new EnlistModel() { ID = 4, Name = "EDUC" });
    }
}
public class EnlistModel
{
    public int ID { get; set; }
    public String? Name { get; set; }

}
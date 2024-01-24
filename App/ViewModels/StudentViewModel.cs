using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class StudentViewModel : ObservableObject
{
    public ObservableCollection<StudentModel> Students { get; } = new ObservableCollection<StudentModel>();
    public StudentViewModel()
    {
        Students.Add(new StudentModel() { ID = 1, Name = "Kalvin" });
        Students.Add(new StudentModel() { ID = 2, Name = "Jovylyn" });
        Students.Add(new StudentModel() { ID = 3, Name = "Adriel" });
        Students.Add(new StudentModel() { ID = 4, Name = "Asaiah" });
    }
}
public class StudentModel
{
    public int ID { get; set; }
    public String? Name { get; set; }
}

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class SubjectViewModel : ObservableObject
{
    public ObservableCollection<SubjectModel> Subjects { get; } = new ObservableCollection<SubjectModel>();
    public SubjectViewModel()
    {
        Subjects.Add(new SubjectModel() { ID = 1, Subject = "English" });
        Subjects.Add(new SubjectModel() { ID = 2, Subject = "Math" });
        Subjects.Add(new SubjectModel() { ID = 3, Subject = "Science" });
        Subjects.Add(new SubjectModel() { ID = 4, Subject = "Recess" });
    }
}
public class SubjectModel
{
    public int ID { get; set; }
    public String? Subject { get; set; }
}

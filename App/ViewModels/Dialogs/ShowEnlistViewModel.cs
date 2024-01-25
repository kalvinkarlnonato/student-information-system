using CommunityToolkit.Mvvm.ComponentModel;
using Library.Contracts;
using Library.Models;
using System.Collections.ObjectModel;

namespace App.ViewModels.Dialogs;

public partial class ShowEnlistViewModel : ObservableObject
{
    private readonly IDatabaseService<SubjectModel> SubjectDatabaseService;

    public ObservableCollection<SubjectModel> Subjects { get; set; }

    public ObservableCollection<SubjectModel> StudentSubjects { get; set; }

    [ObservableProperty]
    private StudentModel student;

    public ShowEnlistViewModel(IDatabaseService<SubjectModel> subjectDatabaseService)
    {
        Subjects = new ObservableCollection<SubjectModel>();
        StudentSubjects = new ObservableCollection<SubjectModel>();
        SubjectDatabaseService = subjectDatabaseService;
        LoadSubjects();

        EnlistViewModel enlistViewModel = App.GetService<EnlistViewModel>();
        student = enlistViewModel.SelectedStudent;

        var subs = Subjects.Where(x => (x.Program == Student.Program) && (x.Year == Student.YearLevel) && (x.Sem == 1)).ToList();
        foreach (var sub in subs)
        {
            StudentSubjects.Add(sub);
        }
    }


    public async void LoadSubjects()
    {
        Subjects.Clear();
        var subs = await SubjectDatabaseService.Get();
        foreach (var sub in subs)
        {
            Subjects.Add(sub);
        }
        await Task.CompletedTask;
    }
}

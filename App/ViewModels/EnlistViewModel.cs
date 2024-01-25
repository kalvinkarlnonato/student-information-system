using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Contracts;
using Library.Models;
using Library.Services;
using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace App.ViewModels;

public partial class EnlistViewModel : ObservableObject
{
    private readonly IDatabaseService<StudentModel> StudentDatabaseService;
    private readonly IDatabaseService<PersonModel> PersonDatabaseService;
    private readonly IDatabaseService<SubjectModel> SubjectDatabaseService;

    public ObservableCollection<StudentModel> Students { get; set; }

    public ObservableCollection<SubjectModel> Subjects { get; set; }

    public ObservableCollection<SubjectModel> StudentSubjects { get; set; }

    [ObservableProperty]
    private StudentModel? selectedStudent;

    public EnlistViewModel(IDatabaseService<StudentModel> studentDatabaseService)
    {
        StudentDatabaseService = studentDatabaseService;
        PersonDatabaseService = App.GetService<IDatabaseService<PersonModel>>();
        SubjectDatabaseService = App.GetService<IDatabaseService<SubjectModel>>();
        Students = new ObservableCollection<StudentModel>();
        Subjects = new ObservableCollection<SubjectModel>();
        StudentSubjects = new ObservableCollection<SubjectModel>();

        LoadStudents();
        LoadSubjects();
        //LoadEnrolled();
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

    public async void LoadStudents()
    {
        Students.Clear();
        var students = await StudentDatabaseService.Get();
        var persons = await PersonDatabaseService.Get();

        foreach (var student in students)
        {
            student.Person = persons.FirstOrDefault(x => x.ID == student.PID) ?? new();
            Students.Add(student);
        }
    }
}
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
    private readonly IDatabaseService<StudentFeeModel> StudentFeeDatabaseService;


    public ObservableCollection<StudentModel> Students { get; set; }

    public ObservableCollection<StudentModel> Enrolled { get; set; }

    [ObservableProperty]
    private StudentModel? selectedStudent;

    public EnlistViewModel(IDatabaseService<StudentModel> studentDatabaseService, IDatabaseService<StudentFeeModel> studentFeeDatabaseService)
    {
        StudentDatabaseService = studentDatabaseService;
        StudentFeeDatabaseService = studentFeeDatabaseService;
        PersonDatabaseService = App.GetService<IDatabaseService<PersonModel>>();
        Students = new ObservableCollection<StudentModel>();
        Enrolled = new ObservableCollection<StudentModel>();
        LoadStudents();
        LoadEnrolled();
        //LoadEnrolled();
    }
    
    public async void LoadEnrolled()
    {
        Enrolled.Clear();
        var enrollees = await StudentFeeDatabaseService.Get();
        var sudentFee = new List<StudentFeeModel>();
        foreach (var enrollee in enrollees) sudentFee.Add(enrollee);
        var enrolledStuds = Students.Where(y=> sudentFee.Select(x=>x.StudentID).Contains(y.ID)).ToList();
        foreach (var en in enrolledStuds)
        {
            en.StudentFee = sudentFee.Where(x=>x.StudentID==en.ID).FirstOrDefault()??new();
            Enrolled.Add(en);
        }
    }

    public async Task LoadStudents()
    {
        Students.Clear();
        var students = await StudentDatabaseService.Get();
        var persons = await PersonDatabaseService.Get();

        foreach (var student in students)
        {
            student.Person = persons.FirstOrDefault(x => x.ID == student.PID) ?? new();
            Students.Add(student);
        }
        await Task.CompletedTask;
    }
}
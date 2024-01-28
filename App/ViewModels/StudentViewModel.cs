using CommunityToolkit.Mvvm.ComponentModel;
using Library.Contracts;
using Library.Models;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class StudentViewModel : ObservableObject
{
    private readonly IDatabaseService<StudentModel> StudentDatabaseService;
    private readonly IDatabaseService<PersonModel> PersonDatabaseService;

    public ObservableCollection<StudentModel> Students { get; }

    public StudentViewModel(IDatabaseService<StudentModel> studentDatabaseService,
                            IDatabaseService<PersonModel> personDatabaseService)
    {
        StudentDatabaseService = studentDatabaseService;
        PersonDatabaseService = personDatabaseService;
        Students = new ObservableCollection<StudentModel>();
        LoadStudents();
    }

    public async void LoadStudents()
    {
        Students.Clear();

        var students = await StudentDatabaseService.Get();
        var persons = await PersonDatabaseService.Get();

        foreach (var student in students)
        {
            student.Person = persons.FirstOrDefault(x => x.ID == student.PID)??new();
            Students.Add(student);
        }

        //Persons = await PersonDatabaseService.Get();


        //var records = people.Join(students, arg => arg.ID, arg => arg.PID, (person, student) =>
        
        //new {
        //    StudentNumber = student.SID,
        //    FirstName = person.FirstName,
        //    MiddleName = person.MiddleName,
        //    LastName = person.LastName,
        //    Ex = person.Ex,
        //    College = student.College,
        //    Course = student.Program,
        //    Major = student.Major,
        //    YearLevel = (DateTime.Now.Year - DateTime.Parse(student.Date).Year).ToString(),
        //    EntryDate = student.Date,
        //    Status = student.Status,
        //    DateOfBirth = person.Birthday!=null? DateTime.Parse(person.Birthday).ToString("dd/MM/yyyy") : "",
        //    Sex = person.Gender,
        //    Address = person.Address,
        //    City = person.City,
        //    Province = person.Province,
        //    Scheme = "OPT-IN"
        //});
        //foreach (var record in records)
        //{
        //    Records.Add(record);
        //}
    }

}
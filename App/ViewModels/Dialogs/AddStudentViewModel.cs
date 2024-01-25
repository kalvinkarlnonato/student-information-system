using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Contracts;
using Library.Models;
using Library.Services;
using System.Collections.ObjectModel;

namespace App.ViewModels.Dialogs
{
    public partial class AddStudentViewModel : ObservableObject
    {
        private readonly IDatabaseService<StudentModel> StudentDatabaseService;
        private readonly IDatabaseService<PersonModel> PersonDatabaseService;

        [ObservableProperty]
        private PersonModel person;

        [ObservableProperty]
        private StudentModel student;

        [ObservableProperty]
        private string? studentId;

        [ObservableProperty]
        private string[] programs;

        [ObservableProperty]
        private string[] gender;

        [ObservableProperty]
        private string[] status;

        [ObservableProperty]
        private DateTime birthday = DateTime.Now;

        [ObservableProperty]
        private string birthString = DateTime.Now.ToString();

        public AddStudentViewModel()
        {
            StudentDatabaseService = App.GetService<IDatabaseService<StudentModel>>();
            PersonDatabaseService = App.GetService<IDatabaseService<PersonModel>>();
            Programs = new string[] { "BSCRIM", "BSIT", "BEED", "BSAGRI" };
            gender = new string[] { "MALE", "FEMALE" };
            status = new string[] { "REGULAR", "IRREGULAR", "TRANSFEREE" };
            person = new PersonModel();
            student = new StudentModel();
            student.SID = DateTime.Now.ToString("yy")+"-";
        }

        [RelayCommand]
        private void Save(object obj)
        {
            Student.College = (Student.Program == "BSCRIM" ? "COLLEGE OF CRIMINAL JUSTICE EDUCATION" : (Student.Program == "BSIT" ? "COLLEGE OF INFORMATION AND COMPUTING SCIENCES" : (Student.Program == "BEED" ? "COLLEGE OF TEACHER EDUCATION" : (Student.Program == "BSAGRI" ? "COLLEGE OF AGRICULTURE" : ""))));
            Person.Birthday = Birthday.Year != DateTime.Now.Year ? Birthday.ToString("yyyy-MM-dd") : null;
            SaveAll();
        }

        private async void SaveAll()
        {
            var p = await PersonDatabaseService.Create(Person);
            Student.PID = p.ID;
            await StudentDatabaseService.Create(Student);
        }

    }
}

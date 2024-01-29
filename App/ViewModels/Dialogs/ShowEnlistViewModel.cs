using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Contracts;
using Library.Models;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Popups;
using ColorCode.Compilation.Languages;
using System.Text;
using Library.Services;
using Library.Helpers;

namespace App.ViewModels.Dialogs;

public partial class ShowEnlistViewModel : ObservableObject
{
    private readonly IDatabaseService<StudentFeeModel> StudentFeeDatabaseService;

    [ObservableProperty]
    private StudentModel student;

    public ShowEnlistViewModel(IDatabaseService<StudentFeeModel> studentFeeDatabaseService)
    {
		StudentFeeDatabaseService = studentFeeDatabaseService;

        EnlistViewModel enlistViewModel = App.GetService<EnlistViewModel>();
        Student = enlistViewModel.SelectedStudent??new();

    }

    [RelayCommand]
    private void Print()
    {
  //      SetupDoc();
		//SaveOnly();
    }

	[RelayCommand]
    private async void SaveOnly()
    {
		//StudentFeeModel FeeForSubject = await StudentFeeDatabaseService.Create(new StudentFeeModel()
		//{
		//	StudentID = Student.ID,
		//	TotalFee = fees.Total,
		//	ProcessedBy = processBy
		//});
  //      foreach (var sub in Subjects)
		//{
		//	await StudentSubjectDatabaseService.Create(new StudentSubjectModel()
		//	{
		//		StudentID = Student.ID,
		//		SubjectID = sub.ID,
		//		FeeID = FeeForSubject.ID,
		//		Sem = Settings.Sem,
		//		AcadYearID = Settings.SY
  //          });
  //      }
    }
    
	[RelayCommand]
    private async Task ToPdf()
    {
		//string str = await SetupDoc();
		//var doc = await renderer.RenderHtmlAsPdfAsync(str);
  //      SaveFile("IronPDF HTML string.pdf", "application/pdf", doc.Stream);
  //      SaveOnly();
    }
    
    
}

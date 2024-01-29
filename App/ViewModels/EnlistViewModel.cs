using App.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Library.Contracts;
using Library.Helpers;
using Library.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace App.ViewModels;

public partial class EnlistViewModel : ObservableObject
{
	private readonly IDatabaseService<SettingModel> SettingDatabaseService;
	private readonly IDatabaseService<StudentModel> StudentDatabaseService;
	private readonly IDatabaseService<PersonModel> PersonDatabaseService;
	private readonly IDatabaseService<SubjectModel> SubjectDatabaseService;
	private readonly IDatabaseService<StudentFeeModel> StudentFeeDatabaseService;
	private readonly IDatabaseService<StudentSubjectModel> StudentSubjectDatabaseService;
	private readonly IPrintingService PrintingService;

	public ObservableCollection<StudentModel> Students { get; set; }

	public ObservableCollection<StudentModel> Enrolled { get; set; }

	public ObservableCollection<SubjectModel> Subjects { get; set; }

    public ObservableCollection<SubjectModel> StudentSubjects { get; set; }

    private FeesModel fees = new FeesModel();

	private SettingModel Settings;

	[ObservableProperty]
	private StudentModel selectedStudent;


	[ObservableProperty]
	private StudentModel selectedEnrolled;

	private ChromePdfRenderer renderer;

	public EnlistViewModel(IDatabaseService<StudentModel> studentDatabaseService,
							IDatabaseService<StudentFeeModel> studentFeeDatabaseService,
							IDatabaseService<SubjectModel> subjectDatabaseService,
							IDatabaseService<SettingModel> settingDatabaseService,
							IDatabaseService<StudentSubjectModel> studentSubjectDatabaseService,
							IPrintingService printingService
		)
	{
		SubjectDatabaseService = subjectDatabaseService;
		StudentDatabaseService = studentDatabaseService;
		StudentFeeDatabaseService = studentFeeDatabaseService;
		SettingDatabaseService = settingDatabaseService;
		StudentSubjectDatabaseService = studentSubjectDatabaseService;
		PrintingService = printingService;
		PersonDatabaseService = App.GetService<IDatabaseService<PersonModel>>();
		Students = new ObservableCollection<StudentModel>();
		Enrolled = new ObservableCollection<StudentModel>();
		Subjects = new ObservableCollection<SubjectModel>();
		StudentSubjects = new ObservableCollection<SubjectModel>();
		LoadSettings();
		LoadStudents();
		LoadSubjects();
		LoadEnrolled();
	}

	public void LoadStudentSubjects()
	{
		fees = new FeesModel();
		StudentSubjects.Clear();
		var subs = Subjects.Where(x => (x.Program == SelectedStudent.Program) && (x.Year == SelectedStudent.YearLevel) && (x.Sem == Settings.Sem)).ToList();
		foreach (var subj in subs)
		{
			StudentSubjects.Add(subj);
			if (subj.IsLab) fees.Laboratory += 60;
			if (subj.IsCom) fees.Computer += 360;
			if (!subj.Code!.Contains("NSTP")) fees.Tuition += (subj.Units * 110);
		}
	}

	public async void LoadSettings()
	{
		var set = await SettingDatabaseService.Get();
		Settings = set.FirstOrDefault() ?? new SettingModel() { Sem = 1, SY = "2023-2024", Signatory = "MR. HITSON DANGATAN", Position = "Campus Registrar" };
	}

	[RelayCommand]
	public async Task LoadEnrolled()
	{
		Enrolled.Clear();
		var _studentfees = await StudentFeeDatabaseService.Get();
        var sudentFees = new List<StudentFeeModel>();
		foreach (var studentfee in _studentfees) sudentFees.Add(studentfee);
        var studentSubs = await StudentSubjectDatabaseService.Get();
		var studentSubjeks = new List<StudentSubjectModel>();
		foreach(var stds in studentSubs)
		{
			stds.Subject = Subjects.Where(s => s.ID == stds.SubjectID).FirstOrDefault() ?? new();
			studentSubjeks.Add(stds);
		}
		var enrolledStuds = Students.Where(y => sudentFees.Select(x => x.StudentID).Contains(y.ID)).ToList();
		foreach (var en in enrolledStuds)
		{
			en.StudentFee = sudentFees.Where(x => x.StudentID == en.ID).FirstOrDefault() ?? new();
			en.Subjects = studentSubjeks.Where(x => (x.StudentID == en.ID && x.Sem==Settings.Sem && x.AcadYearID==Settings.SY)).ToList();
			Enrolled.Add(en);
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
	
	[RelayCommand]
	private async Task PDF()
	{
		var htmlPdf = await SetupDoc();
		using (var pdf = await renderer.RenderHtmlAsPdfAsync(htmlPdf))
		{
			PrintingService.SaveFile($"{SelectedEnrolled!.SID}.pdf", "application/pdf", pdf.Stream);
		}
	}

	[RelayCommand]
	private async Task Print()
	{
        var htmlPdf = await SetupDoc();
        using (var pdf = await renderer.RenderHtmlAsPdfAsync(htmlPdf))
        {
			pdf.Print();
        }
    }

    public async Task Save()
    {
		StudentFeeModel FeeForSubject = await StudentFeeDatabaseService.Create(new StudentFeeModel()
		{
			StudentID = SelectedStudent.ID,
			TotalFee = fees.Total,
			ProcessedBy = UserHelpers.ProccessBy
		});
		foreach (var sub in StudentSubjects)
		{
			await StudentSubjectDatabaseService.Create(new StudentSubjectModel()
			{
				StudentID = SelectedStudent.ID,
				SubjectID = sub.ID,
				FeeID = FeeForSubject.ID,
				Sem = Settings.Sem,
				AcadYearID = Settings.SY
			});
		}
	}

    private static readonly ChromePdfRenderOptions ChromePdfRenderOptions = new()
    {
        PaperSize = IronPdf.Rendering.PdfPaperSize.Custom,
		CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen,
        PrintHtmlBackgrounds = true,
        EnableJavaScript = true,
		MarginBottom=5,
		MarginRight=5,
		MarginLeft=5,
		MarginTop=40,
        Timeout = 60
    };

    private async Task<string> SetupDoc()
    {
        fees = new FeesModel();

        if (SelectedEnrolled != null)
        {
            fees.Athletic = 140;
            fees.Cultural = 60;
            fees.Development = 130;
            fees.Admission = 160;
            fees.Guidance = 60;
            fees.HandBook = 60;
            fees.Library = 200;
            fees.MedicalDental = 120;
            fees.SchoolID = 160;
            if (SelectedEnrolled.YearLevel == 1)
            {
                fees.NSTP = 160;
                fees.Registration = 110;
            }
        }

		renderer = new ChromePdfRenderer()
		{
			RenderingOptions = ChromePdfRenderOptions
		};
        renderer.RenderingOptions.SetCustomPaperSizeInInches(8.5f, 13f);

        renderer.RenderingOptions.HtmlHeader = new HtmlHeaderFooter()
        {
            MaxHeight = 55,
            HtmlFragment = @"<center>
                                <div style='width: 50%; position: fixed;'>
	                                <img style='width: 80px; float: right;padding-right: 123px;' src='logoCSU.png'/>
                                </div>
	                            <p>
		                            <span style='font-size: 7.0pt; line-height: 107%; font-family: 'Century Gothic',sans-serif;text-align: center;'>Republic of the Philippines</span><br/>
		                            <strong><span style='font-size: 11.0pt; line-height: 107%; font-family: 'Century Gothic',sans-serif;'>CAGAYAN STATE UNIVERSITY</span></strong><br/>
		                            <strong><span style='font-size: 10.0pt; line-height: 107%; font-family: 'Poor Richard',serif;'>S O L A N A&nbsp;&nbsp; C A M P U S</span></strong><br/>
		                            <span style='font-size: 7.0pt; line-height: 107%; font-family: 'Century Gothic',sans-serif;'>Iraga, Solana Cagayan, 3503</span>
	                            </p>
                                <p>
		                            <strong><span style='font-size: 12.0pt; line-height: 107%; font-family: 'Aptos',sans-serif;'>Assessment Form</span></strong><br/>
		                            <span style='font-size: 12.0pt; line-height: 107%; font-family: 'Aptos',sans-serif;'>" + Settings.SemText + @"</span>
	                            </p>
                            </center> ",
            BaseUrl = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"/Assets/").AbsoluteUri
        };
        renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter()
        {
            MaxHeight = 40, //millimeters
            HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
            DrawDividerLine = false
        };
        
		StringBuilder myHtml = new StringBuilder();
        //StudentDetails   <style> *{font-family: 'Arial Narrow',sans-serif;}</style>
		
		myHtml.Append(@"
                        <style> *{font-family: 'Arial Narrow',sans-serif; font-size: 13px;}</style>
                        <center>                        
                        <table style='border-collapse: collapse;'>
                            <tbody>
		                        <tr>
			                        <td width='90'>ID Number:</td>
			                        <td width='406'>" + SelectedEnrolled.SID + @"</td>
			                        <td width='104'>College:</td>
			                        <td width='134'>" + SelectedEnrolled.Program + @"</td>
		                            </tr>
		                            <tr>
			                        <td>Name:</td>
			                        <td>" + SelectedEnrolled.Person.ProperName + @"</td>
			                        <td>Major:</td>
			                        <td>" + (string.IsNullOrEmpty(SelectedEnrolled.Major) ? "-" : SelectedEnrolled.Major) + @"</td>
		                            </tr>
		                            <tr>
			                        <td>Course:</td>
			                        <td>" + SelectedEnrolled.Course + @"</td>
			                        <td>Year and Sec</td>
			                        <td>" + SelectedEnrolled.YearLevel + @"</td>
		                            </tr>
		                            <tr>
			                        <td>Curriculum</td>
			                        <td>" + SelectedEnrolled.Program + @" 2023</td>
			                        <td>Gender:</td>
			                        <td>" + SelectedEnrolled.Person.Gender + @"</td>
		                        </tr>
	                        </tbody>
                        </table>
	                    <br/>
                    ");
        //Subjects head
        myHtml.Append(@"
		                <span>-------------------------------------------------------------------------------------------------SUBJECTS----------------------------------------------------------------------------------------------------</span>
                        <table width='759'>
	                        <tbody>
		                        <tr style='font-size: 9.0pt;'>
			                        <td style='width: 71.75pt;'><strong>Class Code:</strong></td>
			                        <td style='width: 58.5pt;'><strong>Subject Code:</strong></td>
			                        <td style='width: 117.0pt;'><strong>Descriptive Title:</strong></td>
			                        <td style='width: 38.5pt;'><strong>Units</strong></td>
			                        <td style='width: 40.5pt;'><strong>Day</strong></td>
			                        <td style='width: 65.35pt;'><strong>Start Time</strong></td>
			                        <td style='width: 69.65pt;'><strong>End Time Room</strong></td>
			                        <td style='width: 65.35pt;'><strong>Teacher</strong></td>
		                        </tr>");



        foreach (var sub in SelectedEnrolled.Subjects)
        {
            myHtml.Append("<tr><td></td><td>  ");
            myHtml.Append(sub.Subject.Code);
            myHtml.Append("</td><td>");
            myHtml.Append(sub.Subject.Description);
            myHtml.Append("</td><td>");
            myHtml.Append(sub.Subject.Units);
            myHtml.Append("</td><td></td><td></td><td></td><td></td></tr>");
            if (sub.Subject.IsLab) fees.Laboratory += 60;
            if (sub.Subject.IsCom) fees.Computer += 360;
            if (!sub.Subject.Code!.Contains("NSTP")) fees.Tuition += sub.Subject.Units * 110;
        }

		//Subjects end
		myHtml.Append(@"
	            </tbody>
            </table>
        ");
        //Fees breakdown
        myHtml.Append(@"
                    <br/><span>-------------------------------------------------------------------------------------------<strong>FEES BREAKDOWN</strong>----------------------------------------------------------------------------------------------</span>
                    <table>
				    <tbody>
				    <tr>
				    </tr>
					    <td width='108'></td>
					    <td width='135'><strong>Description</strong></td>
					    <td width='91'><strong>Amount</strong></td>
				    <tr>");

        myHtml.Append(@" <tr>
					        <td>1</td>
					        <td>Admission Fee</td>
					        <td>" + fees.Admission + @"</td>
				        </tr>
				        <tr>
					        <td>2</td>
					        <td>Athletic Fee</td>
					        <td>" + fees.Athletic + @"</td>
				        </tr>
				        <tr>
					        <td>3</td>
					        <td>Computer Fee</td>
					        <td>" + fees.Computer + @"</td>
				        </tr>
				        <tr>
					        <td>4</td>
					        <td>Cultural Fee</td>
					        <td>" + fees.Cultural + @"</td>
				        </tr>
				        <tr>
					        <td>5</td>
					        <td>Development Fee</td>
					        <td>" + fees.Development + @"</td>
				        </tr>
				        <tr>
					        <td>6</td>
					        <td>Guidance Fee</td>
					        <td>" + fees.Guidance + @"</td>
				        </tr>
				        <tr>
					        <td>7</td>
					        <td>Hand Book Fee</td>
					        <td>" + fees.HandBook + @"</td>
				        </tr>
				        <tr>
					        <td>8</td>
					        <td>Laboratory Fee</td>
					        <td>" + fees.Laboratory + @"</td>
				        </tr>
				        <tr>
					        <td>9</td>
					        <td>Library Fee</td>
					        <td>" + fees.Library + @"</td>
				        </tr>
				        <tr>
					        <td>10</td>
					        <td>Medical and Dental Fee</td>
					        <td>" + fees.MedicalDental + @"</td>
				        </tr>
				        <tr>
					        <td>11</td>
					        <td>NSTP Fee</td>
					        <td>" + fees.NSTP + @"</td>
				        </tr>
				        <tr>
					        <td>12</td>
					        <td>Registration Fee</td>
					        <td>" + fees.Registration + @"</td>
				        </tr>
				        <tr>
					        <td>13</td>
					        <td>School ID Fee</td>
					        <td>" + fees.SchoolID + @"</td>
				        </tr>
				        <tr>
					        <td>14</td>
					        <td>Tuition Fee</td>
					        <td>" + fees.Tuition + @"</td>
				        </tr>
				        <tr>
					        <td></td>
					        <td><strong>Total Assessment:</strong></td>
					        <td><strong>" + fees.Total.ToString("N2") + @"</strong></td>
				        </tr> ");

        myHtml.Append(@"
				</tbody>
				</table>");
        myHtml.Append(@"
                <br/>
				<span>--------------------------------------------------------------------------------------------------<strong>DISCOUNT</strong>-----------------------------------------------------------------------------------------------------</span>
				<table>
					<tbody>
					<tr>
						<td width='116'><strong>Type</strong></td>
						<td width='116'><strong>Code</strong></td>
						<td width='116'><strong>Description</strong></td>
						<td width='116'><strong>Method</strong></td>
						<td width='116'><strong>Value Covered</strong></td>
						<td width='116'><strong>Remarks</strong></td>
					</tr>
					<tr>
					<td></td>
					<td></td>
					<td></td>
					<td>SUB-TOTAL</td>
					<td>Php. " + fees.Total.ToString("N2") + @"</td>
					<td></td>
					</tr>
					<tr>
					<td></td>
					<td></td>
					<td></td>
					<td>DISCOUNTS</td>
					<td>Php. 0.00</td>
					<td></td>
					</tr>
					<tr>
					<td></td>
					<td></td>
					<td></td>
					<td>GRAND TOTAL</td>
					<td>Php. " + fees.Total.ToString("N2") + @"</td>
					<td></td>
					</tr>
					</tbody>
					</table>");
        myHtml.Append(@"
					<br/>
					<span>-----------------------------------------------------------------------------------------<span style=""font-size: small;"">ENROLLMENT DETAILS</span>-------------------------------------------------------------------------------------------</span>					
					<table>
						<tbody>
						<tr>
							<td width='96'>Prelims</td>
							<td width='264'><strong>Php. " + fees.PayFinal.ToString("N2") + @"</strong></td>
							<td width='337'>Date: " + DateTime.Now.ToString("MMMM dd, yyyy") + @"</td>
						</tr>
						<tr>
							<td>Midterms</td>
							<td><strong>Php. " + fees.PayPreMid.ToString("N2") + @"</strong></td>
							<td>Time: " + DateTime.Now.ToString("HH:MM tt") + @"</td>
						</tr>
						<tr>
							<td>Finals</td>
							<td><strong>Php. " + fees.PayPreMid.ToString("N2") + @"</strong></td>
							<td>Processed by: " + SelectedEnrolled.StudentFee.ProcessedBy + @"</td>
						</tr>
						</tbody>
					</table>
					<br/><br/><br/>
		            <strong><span style='font-family: 'Cambria Math',serif;'>" + Settings.Signatory + @"</span></strong><br/>
		            <span style='font-size: 11.0pt; line-height: 107%; font-family: 'Aptos',sans-serif;'>" + Settings.Position + @"</span>");
		await Task.CompletedTask;
		return myHtml.ToString();
    }


}
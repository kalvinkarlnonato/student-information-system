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

namespace App.ViewModels.Dialogs;

public partial class ShowEnlistViewModel : ObservableObject
{
    private readonly IDatabaseService<SubjectModel> SubjectDatabaseService;
    private readonly IDatabaseService<SettingModel> SettingDatabaseService;
    private readonly IDatabaseService<StudentFeeModel> StudentFeeDatabaseService;
    private readonly IDatabaseService<StudentSubjectModel> StudentSubjectDatabaseService;

    public ObservableCollection<SubjectModel> Subjects { get; set; }

    public ObservableCollection<SubjectModel> StudentSubjects { get; set; }

    [ObservableProperty]
    private StudentModel student;

    private ChromePdfRenderer renderer;
	private string processBy = Library.User.FullName.ProccessBy;
    private FeesModel fees = new FeesModel();

	private SettingModel Settings { get; set; }

    public ShowEnlistViewModel( IDatabaseService<SubjectModel> subjectDatabaseService,
								IDatabaseService<SettingModel> settingDatabaseService,
								IDatabaseService<StudentSubjectModel> studentSubjectDatabaseService,
								IDatabaseService<StudentFeeModel> studentFeeDatabaseService)
    {
        Subjects = new ObservableCollection<SubjectModel>();
        StudentSubjects = new ObservableCollection<SubjectModel>();
        SubjectDatabaseService = subjectDatabaseService;
		SettingDatabaseService = settingDatabaseService;
		StudentSubjectDatabaseService = studentSubjectDatabaseService;
		StudentFeeDatabaseService = studentFeeDatabaseService;
		LoadSettings();
        LoadSubjects();

        EnlistViewModel enlistViewModel = App.GetService<EnlistViewModel>();
        student = enlistViewModel.SelectedStudent??new();
        fees.Athletic = 140;
        fees.Cultural = 60;
        fees.Development = 130;
        fees.Admission = 160;
        fees.Guidance = 60;
        fees.HandBook = 60;
        fees.Library = 200;
        fees.MedicalDental = 120;
        fees.SchoolID = 160;
        if(student.YearLevel == 1) {
            fees.NSTP = 160;
            fees.Registration = 110;
        }

        var subs = Subjects.Where(x => (x.Program == Student.Program) && (x.Year == Student.YearLevel) && (x.Sem == Settings.Sem)).ToList();
        foreach (var subj in subs)
        {
            StudentSubjects.Add(subj);
            if (subj.IsLab) fees.Laboratory += 60;
            if (subj.IsCom) fees.Computer += 360;
            if (!subj.Code!.Contains("NSTP")) fees.Tuition += (subj.Units * 110);
        }
    }

    private async void LoadSettings()
    {
		var set = await SettingDatabaseService.Get();
		Settings = set.FirstOrDefault()??new SettingModel() { Sem =1, SY="2023-2024", Signatory= "MR. HITSON DANGATAN",Position="Campus Registrar" };
		
	}

    [RelayCommand]
    private void Print()
    {
        SetupDoc().Print();
		SaveOnly();
    }

    [RelayCommand]
    private async void SaveOnly()
    {
		StudentFeeModel FeeForSubject = await StudentFeeDatabaseService.Create(new StudentFeeModel()
		{
			StudentID = Student.ID,
			TotalFee = fees.Total,
			ProcessedBy = processBy
		});
        foreach (var sub in Subjects)
		{
			await StudentSubjectDatabaseService.Create(new StudentSubjectModel()
			{
				StudentID = Student.ID,
				SubjectID = sub.ID,
				FeeID = FeeForSubject.ID,
				Sem = Settings.Sem,
				AcadYearID = Settings.SY
            });
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
    
	[RelayCommand]
    private void ToPdf()
    {
        SaveFile("IronPDF HTML string.pdf", "application/pdf", SetupDoc().Stream);
        SaveOnly();
    }

    private PdfDocument SetupDoc()
    {
        renderer = new ChromePdfRenderer();
        renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.Custom;
        renderer.RenderingOptions.SetCustomPaperSizeInInches(8.5f, 13f);
        renderer.RenderingOptions.MarginTop = 40;
        renderer.RenderingOptions.MarginLeft = 5;
        renderer.RenderingOptions.MarginRight = 5;
        renderer.RenderingOptions.MarginBottom = 5;
        renderer.RenderingOptions.HtmlHeader = new HtmlHeaderFooter(){
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
		                            <span style='font-size: 12.0pt; line-height: 107%; font-family: 'Aptos',sans-serif;'>"+ Settings.SemText + @"</span>
	                            </p>
                            </center> ",
            BaseUrl = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"/Assets/").AbsoluteUri
        };
        renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter(){
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
			                        <td width='406'>" + Student.SID+@"</td>
			                        <td width='104'>College:</td>
			                        <td width='134'>"+Student.Program+@"</td>
		                            </tr>
		                            <tr>
			                        <td>Name:</td>
			                        <td>" + Student.Person.ProperName + @"</td>
			                        <td>Major:</td>
			                        <td>" + (string.IsNullOrEmpty(Student.Major) ? "-" : Student.Major) + @"</td>
		                            </tr>
		                            <tr>
			                        <td>Course:</td>
			                        <td>" + Student.Course + @"</td>
			                        <td>Year and Sec</td>
			                        <td>" + Student.YearLevel + @"</td>
		                            </tr>
		                            <tr>
			                        <td>Curriculum</td>
			                        <td>" + Student.Program + @" 2023</td>
			                        <td>Gender:</td>
			                        <td>" + Student.Person.Gender + @"</td>
		                        </tr>
	                        </tbody>
                        </table>
	                    <br/>
                    ");
        //Subjects head
        myHtml.Append(@"
		                <span>-------------------------------------------------------------------------------------------------SUBJECTS----------------------------------------------------------------------------------------------------</span>
		                <br/><br/>
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

        foreach(var subj in StudentSubjects)
        {
            myHtml.Append("<tr><td></td><td>  ");
            myHtml.Append(subj.Code);
            myHtml.Append("</td><td>");
            myHtml.Append(subj.Description);
            myHtml.Append("</td><td>");
            myHtml.Append(subj.Units);
            myHtml.Append("</td><td></td><td></td><td></td><td></td></tr>");
        }
        //Subjects end
        myHtml.Append(@"
	            </tbody>
            </table>
        ");
        //Fees breakdown
        myHtml.Append(@"
                    <br/><span>-------------------------------------------------------------------------------------------<strong>FEES BREAKDOWN</strong>----------------------------------------------------------------------------------------------</span>
                    <br/><br/>
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
					        <td>"+fees.Admission+@"</td>
				        </tr>
				        <tr>
					        <td>2</td>
					        <td>Athletic Fee</td>
					        <td>"+fees.Athletic+@"</td>
				        </tr>
				        <tr>
					        <td>3</td>
					        <td>Computer Fee</td>
					        <td>"+fees.Computer+@"</td>
				        </tr>
				        <tr>
					        <td>4</td>
					        <td>Cultural Fee</td>
					        <td>"+fees.Cultural+@"</td>
				        </tr>
				        <tr>
					        <td>5</td>
					        <td>Development Fee</td>
					        <td>"+fees.Development+@"</td>
				        </tr>
				        <tr>
					        <td>6</td>
					        <td>Guidance Fee</td>
					        <td>"+fees.Guidance+@"</td>
				        </tr>
				        <tr>
					        <td>7</td>
					        <td>Hand Book Fee</td>
					        <td>"+fees.HandBook+@"</td>
				        </tr>
				        <tr>
					        <td>8</td>
					        <td>Laboratory Fee</td>
					        <td>"+fees.Laboratory+@"</td>
				        </tr>
				        <tr>
					        <td>9</td>
					        <td>Library Fee</td>
					        <td>"+fees.Library+@"</td>
				        </tr>
				        <tr>
					        <td>10</td>
					        <td>Medical and Dental Fee</td>
					        <td>"+fees.MedicalDental+@"</td>
				        </tr>
				        <tr>
					        <td>11</td>
					        <td>NSTP Fee</td>
					        <td>"+fees.NSTP+@"</td>
				        </tr>
				        <tr>
					        <td>12</td>
					        <td>Registration Fee</td>
					        <td>"+fees.Registration+@"</td>
				        </tr>
				        <tr>
					        <td>13</td>
					        <td>School ID Fee</td>
					        <td>"+fees.SchoolID+@"</td>
				        </tr>
				        <tr>
					        <td>14</td>
					        <td>Tuition Fee</td>
					        <td>"+fees.Tuition+@"</td>
				        </tr>
				        <tr>
					        <td></td>
					        <td><strong>Total Assessment:</strong></td>
					        <td><strong>"+fees.Total.ToString("N2") + @"</strong></td>
				        </tr> ");

        myHtml.Append(@"
				</tbody>
				</table>");
        myHtml.Append(@"
                <br/>
				<span>--------------------------------------------------------------------------------------------------<strong>DISCOUNT</strong>-----------------------------------------------------------------------------------------------------</span>
				<br/><br/>
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
					<td>Php. " + fees.Total.ToString("N2")+@"</td>
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
					<td>Php. "+fees.Total.ToString("N2")+@"</td>
					<td></td>
					</tr>
					</tbody>
					</table>");
        myHtml.Append(@"
					<br/>
					<span>-----------------------------------------------------------------------------------------<span style=""font-size: small;"">ENROLLMENT DETAILS</span>-------------------------------------------------------------------------------------------</span>
					<br/><br/>						
					<table>
						<tbody>
						<tr>
							<td width='96'>Prelims</td>
							<td width='264'><strong>Php. " + fees.PayFinal.ToString("N2")+@"</strong></td>
							<td width='337'>Date: "+DateTime.Now.ToString("MMMM dd, yyyy")+@"</td>
						</tr>
						<tr>
							<td>Midterms</td>
							<td><strong>Php. "+fees.PayPreMid.ToString("N2")+@"</strong></td>
							<td>Time: "+DateTime.Now.ToString("HH:MM tt")+@"</td>
						</tr>
						<tr>
							<td>Finals</td>
							<td><strong>Php. "+fees.PayPreMid.ToString("N2")+@"</strong></td>
							<td>Processed by: "+processBy+ @"</td>
						</tr>
						</tbody>
					</table>
					<br/><br/><br/>
		            <strong><span style='font-family: 'Cambria Math',serif;'>" + Settings.Signatory + @"</span></strong><br/>
		            <span style='font-size: 11.0pt; line-height: 107%; font-family: 'Aptos',sans-serif;'>" + Settings.Position + @"</span>");
        return renderer.RenderHtmlAsPdf(myHtml.ToString());
    }

    private async void SaveFile(string filename, string contentType, MemoryStream stream)
    {
        StorageFile stFile;
        string extension = Path.GetExtension(filename);
        //Gets process windows handle to open the dialog in application process. 
        IntPtr windowHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
        {
            //Creates file save picker to save a file. 
            FileSavePicker savePicker = new();
            savePicker.DefaultFileExtension = ".pdf";
            savePicker.SuggestedFileName = filename;
            //Saves the file as Pdf file.
            savePicker.FileTypeChoices.Add("PDF", new List<string>() { ".pdf" });
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, windowHandle);
            stFile = await savePicker.PickSaveFileAsync();
        }
        else
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }
        if (stFile != null)
        {
            using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                //Writes compressed data from memory to file.
                using Stream outstream = zipStream.AsStreamForWrite();
                outstream.SetLength(0);
                //Saves the stream as file.
                byte[] buffer = stream.ToArray();
                outstream.Write(buffer, 0, buffer.Length);
                outstream.Flush();
            }
            //Create message dialog box. 
            MessageDialog msgDialog = new("Do you want to view the document?", "File has been created successfully");
            UICommand yesCmd = new("Yes");
            msgDialog.Commands.Add(yesCmd);
            UICommand noCmd = new("No");
            msgDialog.Commands.Add(noCmd);
            WinRT.Interop.InitializeWithWindow.Initialize(msgDialog, windowHandle);
            //Showing a dialog box. 
            IUICommand cmd = await msgDialog.ShowAsync();
            if (cmd.Label == yesCmd.Label)
            {
                //Launch the saved file. 
                await Windows.System.Launcher.LaunchFileAsync(stFile);
            }
        }
    }
}

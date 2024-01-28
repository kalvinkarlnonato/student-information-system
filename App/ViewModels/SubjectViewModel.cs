using App.Contracts.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace App.ViewModels;

public partial class SubjectViewModel : ObservableObject
{
    public IAddSampleService SampleService { get; }

    [RelayCommand]
    private void ShowDialog()
    {
        SampleService.ShowDialog();
    }

    public SubjectViewModel(IAddSampleService sampleService)
    {
        SampleService = sampleService;
    }
}

using Salute.ViewModels;

namespace Salute.Views.Patients;

public partial class PatientDetailPage : ContentPage
{
    private readonly PatientDetailViewModel _viewModel;

    public PatientDetailPage(PatientDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public async Task LoadPatient(int id)
    {
        await _viewModel.LoadPatientAsync(id);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
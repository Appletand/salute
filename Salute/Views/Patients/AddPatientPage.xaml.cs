using Salute.ViewModels;

namespace Salute.Views.Patients;

public partial class AddPatientPage : ContentPage
{
    private readonly AddPatientViewModel _viewModel;

    public AddPatientPage(AddPatientViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnSaved += OnPatientSaved;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnSaved -= OnPatientSaved;
    }

    private async void OnPatientSaved()
    {
        await Shell.Current.GoToAsync("..");
    }
}
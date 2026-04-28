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
        _viewModel.OnSaved += async () =>
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Navigation.PopAsync());
    }
}
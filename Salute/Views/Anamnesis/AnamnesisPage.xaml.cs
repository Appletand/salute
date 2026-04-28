using Salute.ViewModels;

namespace Salute.Views.Anamnesis;

public partial class AnamnesisPage : ContentPage
{
    private readonly AnamnesisViewModel _viewModel;

    public AnamnesisPage(AnamnesisViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadPatientsCommand.Execute(null);
    }

    private void OnPatientSelected(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is Salute.Models.Patient patient)
            _viewModel.SelectedPatientId = patient.Id;
    }
}
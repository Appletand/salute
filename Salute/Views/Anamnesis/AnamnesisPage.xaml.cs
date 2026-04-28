using Salute.ViewModels;
using Microsoft.Maui.Controls;

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

    private async void OnPatientSelected(object sender, EventArgs e)
    {
        // O Command já lida com a seleção via SelectedPatientId setter
        await Task.CompletedTask;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Se existir um comando LoadPatientsCommand, executa
        if (_viewModel.LoadPatientsCommand?.CanExecute(null) == true)
            _viewModel.LoadPatientsCommand.Execute(null);
        else if (_viewModel.LoadPatientsCommand != null)
            _viewModel.LoadPatientsCommand.Execute(null);
    }
}

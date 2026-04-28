using Salute.Models;
using Salute.ViewModels;

namespace Salute.Views.Patients;

public partial class PatientsPage : ContentPage
{
    private readonly PatientsViewModel _viewModel;

    public PatientsPage(PatientsViewModel viewModel)
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

    private async void OnAddPatientClicked(object sender, EventArgs e)
    {
        var page = Handler?.MauiContext?.Services.GetService<AddPatientPage>();
        if (page != null)
            await Navigation.PushAsync(page);
    }

    private async void OnViewPatientClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Patient patient)
        {
            var page = Handler?.MauiContext?.Services.GetService<PatientDetailPage>();
            if (page != null)
            {
                await Navigation.PushAsync(page);
                await page.LoadPatient(patient.Id);
            }
        }
    }
}
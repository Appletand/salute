using Salute.Models;
using Salute.ViewModels;

namespace Salute.Views.Patients;

public partial class PatientsPage : ContentPage
{
    private readonly PatientsViewModel _viewModel;

    public PatientsPage(PatientsViewModel viewModel)
    {
        try
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO PatientsPage constructor: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("PatientsPage apareceu");
        try
        {
            _viewModel.LoadPatientsCommand.Execute(null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO LoadPatients: {ex.Message}");
        }
    }

    private async void OnAddPatientClicked(object sender, EventArgs e)
    {
        try
        {
            var page = Handler?.MauiContext?.Services.GetService<AddPatientPage>();
            if (page != null)
                await Navigation.PushAsync(page);
            else
                Console.WriteLine("ERRO: AddPatientPage não resolvida pelo DI");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO OnAddPatientClicked: {ex.Message}");
        }
    }

    private async void OnViewPatientClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button btn && btn.CommandParameter is Patient patient)
            {
                var page = Handler?.MauiContext?.Services.GetService<PatientDetailPage>();
                if (page != null)
                {
                    await Navigation.PushAsync(page);
                    await page.LoadPatient(patient.Id);
                }
                else
                    Console.WriteLine("ERRO: PatientDetailPage não resolvida pelo DI");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO OnViewPatientClicked: {ex.Message}");
        }
    }
}
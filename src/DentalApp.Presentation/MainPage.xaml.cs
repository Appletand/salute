namespace DentalApp.Presentation;

public partial class MainPage : ContentPage
{
    private readonly IServiceProvider _services;

    public MainPage(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
    }

    private async void OnPatientsClicked(object sender, EventArgs e)
    {
        // Navegar para página de pacientes
        await Navigation.PushAsync(_services.GetRequiredService<PatientsPage>());
    }

    private void OnAppointmentsClicked(object sender, EventArgs e) { }
    private void OnExportClicked(object sender, EventArgs e) { }
}
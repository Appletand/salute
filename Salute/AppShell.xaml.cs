using Salute.Views.Patients;
using Salute.Views.Anamnesis;

namespace Salute;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PatientsPage), typeof(PatientsPage));
        Routing.RegisterRoute(nameof(AnamnesisPage), typeof(AnamnesisPage));
        Routing.RegisterRoute(nameof(PatientDetailPage), typeof(PatientDetailPage));
        Routing.RegisterRoute(nameof(AddPatientPage), typeof(AddPatientPage));
    }
}
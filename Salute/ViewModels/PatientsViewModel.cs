using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Salute.Interfaces;
using Salute.Models;
using Salute.Services;

namespace Salute.ViewModels;

public class PatientsViewModel : INotifyPropertyChanged
{
    private readonly IPatientRepository _patientRepository;
    private readonly NavigationState _navigationState;

    private string _statusMessage = string.Empty;
    private bool _isLoading;

    public ObservableCollection<Patient> Patients { get; } = new();

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ICommand LoadPatientsCommand { get; }
    public ICommand AddPatientCommand { get; }
    public ICommand SelectPatientCommand { get; }

    public PatientsViewModel(IPatientRepository patientRepository, NavigationState navigationState)
    {
        _patientRepository = patientRepository;
        _navigationState = navigationState;

        LoadPatientsCommand = new Command(async () => await LoadPatientsAsync());
        AddPatientCommand = new Command(async () => await Shell.Current.GoToAsync("AddPatientPage"));
        SelectPatientCommand = new Command<Patient>(async (patient) => await SelectPatientAsync(patient));
    }

    public async Task LoadPatientsAsync()
    {
        try
        {
            IsLoading = true;
            var result = await _patientRepository.GetAllAsync();
            Patients.Clear();
            foreach (var p in result)
                Patients.Add(p);

            StatusMessage = Patients.Count == 0 ? "Nenhum paciente cadastrado." : string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erro ao carregar pacientes: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SelectPatientAsync(Patient patient)
    {
        _navigationState.SelectedPatientId = patient.Id;
        await Shell.Current.GoToAsync("PatientDetailPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
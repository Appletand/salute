using System.Collections.ObjectModel;
using System.Windows.Input;
using Salute.Models;
using Salute.Interfaces;

namespace Salute.ViewModels;

public class PatientsViewModel
{
    private readonly IPatientRepository _patientRepository;

    public ObservableCollection<Patient> Patients { get; set; }
    public ICommand LoadPatientsCommand { get; }
    public ICommand AddPatientCommand { get; }

    public PatientsViewModel(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
        Patients = new ObservableCollection<Patient>();
        LoadPatientsCommand = new Command(async () => await LoadPatients());
        AddPatientCommand = new Command(async () => await AddDummyPatient());

        Task.Run(async () => await LoadPatients());
    }

    private async Task LoadPatients()
    {
        var patientsFromDb = await _patientRepository.GetAllAsync();
        Patients.Clear();
        foreach (var p in patientsFromDb)
            Patients.Add(p);
    }

    private async Task AddDummyPatient()
    {
        var newPatient = new Patient
        {
            FirstName = "Teste",
            LastName = "DB",
            Email = "teste@db.com",
            Phone = "(11) 90000-0000"
        };
        await _patientRepository.AddAsync(newPatient);
        await LoadPatients();
    }
}
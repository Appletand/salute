using System.Collections.ObjectModel;
using System.Windows.Input;
using Salute.Models;
using Salute.Interfaces;

namespace Salute.ViewModels;

public class PatientsViewModel
{
    private readonly IPatientRepository _patientRepository;

    public ObservableCollection<Patient> Patients { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public ICommand LoadPatientsCommand { get; }
    public ICommand AddPatientCommand { get; }

    public PatientsViewModel(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
        Patients = new ObservableCollection<Patient>();
        LoadPatientsCommand = new Command(async () => await LoadPatients());
        AddPatientCommand = new Command(async () => await AddDummyPatient());
    }

    private async Task LoadPatients()
    {
        try
        {
            Console.WriteLine("LoadPatients: iniciando...");
            var patientsFromDb = await _patientRepository.GetAllAsync();
            Console.WriteLine($"LoadPatients: {patientsFromDb.Count()} pacientes encontrados");
            Patients.Clear();
            foreach (var p in patientsFromDb)
                Patients.Add(p);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO LoadPatients: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private async Task AddDummyPatient()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO AddDummyPatient: {ex.Message}");
        }
    }
}
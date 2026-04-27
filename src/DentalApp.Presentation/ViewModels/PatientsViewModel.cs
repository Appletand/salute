using System.Collections.ObjectModel;
using System.Windows.Input;
using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;

namespace DentalApp.Presentation.ViewModels;

public class PatientsViewModel : BindableObject
{
    private readonly IPatientRepository _repository;
    public ObservableCollection<Patient> Patients { get; set; } = new();

    public ICommand LoadCommand { get; }

    public PatientsViewModel(IPatientRepository repository)
    {
        _repository = repository;
        LoadCommand = new Command(async () => await LoadPatients());
    }

    private async Task LoadPatients()
    {
        var patients = await _repository.GetAllAsync();
        Patients.Clear();
        foreach (var p in patients) Patients.Add(p);
    }
}
}
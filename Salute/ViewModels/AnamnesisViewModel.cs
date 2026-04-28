using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Salute.Interfaces;
using Salute.Models;

namespace Salute.ViewModels;

public class AnamnesisViewModel : INotifyPropertyChanged
{
    private readonly IAnamnesisRepository _anamnesisRepository;
    private readonly IPatientRepository _patientRepository;

    private Anamnesis _anamnesis = new();
    private ObservableCollection<Anamnesis> _anamnesisHistory = new();
    private bool _isLoading;
    private string _statusMessage = string.Empty;
    private int _selectedPatientId;
    private ObservableCollection<Patient> _patients = new();

    public AnamnesisViewModel(IAnamnesisRepository anamnesisRepository, IPatientRepository patientRepository)
    {
        _anamnesisRepository = anamnesisRepository;
        _patientRepository = patientRepository;

        LoadPatientsCommand = new Command(async () => await LoadPatientsAsync());
        SaveCommand = new Command(async () => await SaveAsync(), () => !IsLoading);
        LoadHistoryCommand = new Command<int>(async (id) => await LoadHistoryAsync(id));
        ClearCommand = new Command(ClearForm);

        Task.Run(async () => await LoadPatientsAsync());
    }

    public Anamnesis Anamnesis { get => _anamnesis; set { _anamnesis = value; OnPropertyChanged(); } }
    public ObservableCollection<Anamnesis> AnamnesisHistory { get => _anamnesisHistory; set { _anamnesisHistory = value; OnPropertyChanged(); } }
    public ObservableCollection<Patient> Patients { get => _patients; set { _patients = value; OnPropertyChanged(); } }
    public int SelectedPatientId { get => _selectedPatientId; set { _selectedPatientId = value; OnPropertyChanged(); if (value > 0) LoadHistoryCommand.Execute(value); } }
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); ((Command)SaveCommand).ChangeCanExecute(); } }
    public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }

    public ICommand LoadPatientsCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand LoadHistoryCommand { get; }
    public ICommand ClearCommand { get; }

    private async Task LoadPatientsAsync()
    {
        try
        {
            IsLoading = true;
            var patients = await _patientRepository.GetAllAsync();
            Patients.Clear();
            foreach (var p in patients) Patients.Add(p);
        }
        catch (Exception ex) { StatusMessage = $"Erro: {ex.Message}"; }
        finally { IsLoading = false; }
    }

    private async Task LoadHistoryAsync(int patientId)
    {
        try
        {
            IsLoading = true;
            var history = await _anamnesisRepository.GetByPatientIdAsync(patientId);
            AnamnesisHistory.Clear();
            foreach (var item in history) AnamnesisHistory.Add(item);
            StatusMessage = $"{AnamnesisHistory.Count} anamneses encontradas";
        }
        catch (Exception ex) { StatusMessage = $"Erro: {ex.Message}"; }
        finally { IsLoading = false; }
    }

    private async Task SaveAsync()
    {
        if (SelectedPatientId <= 0) { StatusMessage = "Selecione um paciente!"; return; }
        if (string.IsNullOrWhiteSpace(Anamnesis.MainComplaint)) { StatusMessage = "Queixa principal obrigatoria!"; return; }
        try
        {
            IsLoading = true;
            Anamnesis.PatientId = SelectedPatientId;
            if (Anamnesis.Id == 0) await _anamnesisRepository.AddAsync(Anamnesis);
            else await _anamnesisRepository.UpdateAsync(Anamnesis);
            StatusMessage = "Salvo com sucesso!";
            await LoadHistoryAsync(SelectedPatientId);
        }
        catch (Exception ex) { StatusMessage = $"Erro: {ex.Message}"; }
        finally { IsLoading = false; }
    }

    private void ClearForm() { Anamnesis = new Anamnesis(); StatusMessage = "Formulario limpo."; }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

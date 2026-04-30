using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Salute.Features.AIAssistant;
using Salute.Interfaces;
using Salute.Models;

namespace Salute.ViewModels;

public class AnamnesisViewModel : INotifyPropertyChanged
{
    private readonly IAnamnesisRepository _anamnesisRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly GenerateAnamnesisHandler _aiHandler;

    private Anamnesis _anamnesis = new();
    private ObservableCollection<Anamnesis> _anamnesisHistory = new();
    private bool _isLoading;
    private string _statusMessage = string.Empty;
    private string _aiResult = string.Empty;
    private bool _isAiResultVisible;
    private int _selectedPatientId;
    private ObservableCollection<Patient> _patients = new();

    public AnamnesisViewModel(
        IAnamnesisRepository anamnesisRepository,
        IPatientRepository patientRepository,
        IGeminiService geminiService)
    {
        _anamnesisRepository = anamnesisRepository;
        _patientRepository = patientRepository;
        _aiHandler = new GenerateAnamnesisHandler(geminiService);

        LoadPatientsCommand = new Command(async () => await LoadPatientsAsync());
        SaveCommand = new Command(async () => await SaveAsync(), () => !IsLoading);
        LoadHistoryCommand = new Command<int>(async (id) => await LoadHistoryAsync(id));
        ClearCommand = new Command(ClearForm);
        GenerateAnamnesisCommand = new Command(async () => await GenerateAnamnesisAsync(), () => !IsLoading);
        SuggestDiagnosisCommand = new Command(async () => await SuggestDiagnosisAsync(), () => !IsLoading);

        Task.Run(async () => await LoadPatientsAsync());
    }

    public Anamnesis Anamnesis { get => _anamnesis; set { _anamnesis = value; OnPropertyChanged(); } }
    public ObservableCollection<Anamnesis> AnamnesisHistory { get => _anamnesisHistory; set { _anamnesisHistory = value; OnPropertyChanged(); } }
    public ObservableCollection<Patient> Patients { get => _patients; set { _patients = value; OnPropertyChanged(); } }
    public int SelectedPatientId { get => _selectedPatientId; set { _selectedPatientId = value; OnPropertyChanged(); if (value > 0) LoadHistoryCommand.Execute(value); } }
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); ((Command)SaveCommand).ChangeCanExecute(); ((Command)GenerateAnamnesisCommand).ChangeCanExecute(); ((Command)SuggestDiagnosisCommand).ChangeCanExecute(); } }
    public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }
    public string AiResult { get => _aiResult; set { _aiResult = value; OnPropertyChanged(); } }
    public bool IsAiResultVisible { get => _isAiResultVisible; set { _isAiResultVisible = value; OnPropertyChanged(); } }

    public ICommand LoadPatientsCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand LoadHistoryCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand GenerateAnamnesisCommand { get; }
    public ICommand SuggestDiagnosisCommand { get; }

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
            StatusMessage = $"{AnamnesisHistory.Count} anamnese(s) encontrada(s)";
        }
        catch (Exception ex) { StatusMessage = $"Erro: {ex.Message}"; }
        finally { IsLoading = false; }
    }

    private async Task SaveAsync()
    {
        if (SelectedPatientId <= 0) { StatusMessage = "Selecione um paciente!"; return; }
        if (string.IsNullOrWhiteSpace(Anamnesis.MainComplaint)) { StatusMessage = "Queixa principal obrigatória!"; return; }
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

    private async Task GenerateAnamnesisAsync()
    {
        if (SelectedPatientId <= 0) { StatusMessage = "Selecione um paciente!"; return; }
        try
        {
            IsLoading = true;
            StatusMessage = "Gerando anamnese com IA...";
            var patient = Patients.FirstOrDefault(p => p.Id == SelectedPatientId);
            var command = new GenerateAnamnesisCommand(
                PatientName: $"{patient?.FirstName} {patient?.LastName}",
                MedicalHistory: Anamnesis.MedicalConditions,
                MainSymptoms: Anamnesis.MainComplaint);
            var response = await _aiHandler.Handle(command);
            AiResult = response.SuggestedAnamnesis;
            IsAiResultVisible = true;
            StatusMessage = "Anamnese gerada com sucesso!";
        }
        catch (Exception ex) { StatusMessage = $"Erro IA: {ex.Message}"; }
        finally { IsLoading = false; }
    }

    private async Task SuggestDiagnosisAsync()
    {
        if (string.IsNullOrWhiteSpace(Anamnesis.MainComplaint)) { StatusMessage = "Preencha a queixa principal!"; return; }
        try
        {
            IsLoading = true;
            StatusMessage = "Analisando hipóteses diagnósticas...";
            var summary = $"Queixa: {Anamnesis.MainComplaint}. Sintomas: {Anamnesis.Symptoms}. Histórico: {Anamnesis.MedicalConditions}. Alergias: {Anamnesis.Allergies}.";
            var vitalSigns = $"PA: {Anamnesis.BloodPressureSystolic}/{Anamnesis.BloodPressureDiastolic}, FC: {Anamnesis.HeartRate}, Temp: {Anamnesis.Temperature}°C";
            var command = new GenerateDiagnosisCommand(summary, vitalSigns);
            var response = await _aiHandler.HandleDiagnosis(command);
            AiResult = response.Suggestions;
            IsAiResultVisible = true;
            StatusMessage = "Hipóteses geradas!";
        }
        catch (Exception ex) { StatusMessage = $"Erro IA: {ex.Message}"; }
        finally { IsLoading = false; }
    }

    private void ClearForm()
    {
        Anamnesis = new Anamnesis();
        AiResult = string.Empty;
        IsAiResultVisible = false;
        StatusMessage = "Formulário limpo.";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
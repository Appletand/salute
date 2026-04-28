using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Salute.Interfaces;
using Salute.Models;

namespace Salute.ViewModels;

public class PatientDetailViewModel : INotifyPropertyChanged
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAnamnesisRepository _anamnesisRepository;

    private Patient _patient = new();
    private bool _isSensitiveVisible;
    private bool _isLoading;
    private string _initials = string.Empty;

    public Patient Patient
    {
        get => _patient;
        set { _patient = value; OnPropertyChanged(); UpdateInitials(); }
    }

    public string Initials
    {
        get => _initials;
        set { _initials = value; OnPropertyChanged(); }
    }

    public bool IsSensitiveVisible
    {
        get => _isSensitiveVisible;
        set { _isSensitiveVisible = value; OnPropertyChanged(); OnPropertyChanged(nameof(SensitiveToggleText)); }
    }

    public string SensitiveToggleText => IsSensitiveVisible ? "Ocultar dados sensíveis" : "Exibir dados sensíveis";

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public ObservableCollection<TimelineItem> Timeline { get; set; } = new();

    public ICommand ToggleSensitiveCommand { get; }
    public ICommand LoadCommand { get; }

    public PatientDetailViewModel(IPatientRepository patientRepository, IAnamnesisRepository anamnesisRepository)
    {
        _patientRepository = patientRepository;
        _anamnesisRepository = anamnesisRepository;

        ToggleSensitiveCommand = new Command(() => IsSensitiveVisible = !IsSensitiveVisible);
        LoadCommand = new Command<int>(async (id) => await LoadAsync(id));
    }

    public async Task LoadAsync(int patientId)
    {
        try
        {
            IsLoading = true;
            Timeline.Clear();

            // Carrega dados do paciente
            var patients = await _patientRepository.GetAllAsync();
            Patient = patients.FirstOrDefault(p => p.Id == patientId) ?? new Patient();

            // Anamneses
            var anamneses = await _anamnesisRepository.GetByPatientIdAsync(patientId);
            foreach (var a in anamneses)
            {
                Timeline.Add(new TimelineItem
                {
                    Date = a.CreatedAt,
                    Type = TimelineItemType.Anamnesis,
                    Title = "Anamnese",
                    Summary = a.MainComplaint
                });
            }

            // Appointments e ClinicalNotes virão aqui quando os repositórios existirem

            var sorted = Timeline.OrderByDescending(t => t.Date).ToList();
            Timeline.Clear();
            foreach (var item in sorted)
                Timeline.Add(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar paciente: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void UpdateInitials()
    {
        var first = Patient.FirstName?.FirstOrDefault().ToString() ?? "";
        var last = Patient.LastName?.FirstOrDefault().ToString() ?? "";
        Initials = $"{first}{last}".ToUpper();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
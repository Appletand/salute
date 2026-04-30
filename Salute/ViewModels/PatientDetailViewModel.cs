using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Salute.Interfaces;
using Salute.Models;
using Salute.Services;

namespace Salute.ViewModels;

public class PatientDetailViewModel : INotifyPropertyChanged
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAnamnesisRepository _anamnesisRepository;
    private readonly NavigationState _navigationState;

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

    public PatientDetailViewModel(
        IPatientRepository patientRepository,
        IAnamnesisRepository anamnesisRepository,
        NavigationState navigationState)
    {
        _patientRepository = patientRepository;
        _anamnesisRepository = anamnesisRepository;
        _navigationState = navigationState;

        ToggleSensitiveCommand = new Command(() => IsSensitiveVisible = !IsSensitiveVisible);
        LoadCommand = new Command(async () => await LoadAsync());
    }

    public async Task LoadPatientAsync(int id)
    {
        _navigationState.SelectedPatientId = id;
        await LoadAsync();
    }

    public async Task LoadAsync()
    {
        var patientId = _navigationState.SelectedPatientId;
        if (patientId is null) return;

        try
        {
            IsLoading = true;
            Timeline.Clear();

            var patients = await _patientRepository.GetAllAsync();
            Patient = patients.FirstOrDefault(p => p.Id == patientId) ?? new Patient();

            var anamneses = await _anamnesisRepository.GetByPatientIdAsync(patientId.Value);
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
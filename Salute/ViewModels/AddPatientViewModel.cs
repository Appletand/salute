using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Salute.Interfaces;
using Salute.Models;

namespace Salute.ViewModels;

public class AddPatientViewModel : INotifyPropertyChanged
{
    private readonly IPatientRepository _patientRepository;

    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _phone = string.Empty;
    private string _email = string.Empty;
    private DateTime _dateOfBirth = DateTime.Today.AddYears(-30);
    private bool _isLoading;
    private string _statusMessage = string.Empty;

    public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(); } }
    public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(); } }
    public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
    public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
    public DateTime DateOfBirth { get => _dateOfBirth; set { _dateOfBirth = value; OnPropertyChanged(); } }
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }
    public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }

    public event Action? OnSaved;

    public AddPatientViewModel(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
        SaveCommand = new Command(async () => await SaveAsync(), () => !IsLoading);
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            StatusMessage = "Nome e sobrenome são obrigatórios.";
            return;
        }

        try
        {
            IsLoading = true;
            var patient = new Patient
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                Phone = Phone.Trim(),
                Email = Email.Trim(),
                DateOfBirth = DateOfBirth
            };
            await _patientRepository.AddAsync(patient);
            StatusMessage = "Paciente cadastrado com sucesso!";
            OnSaved?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erro: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
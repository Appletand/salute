using SaluteWeb.Interfaces;
using SaluteWeb.Models;

namespace SaluteWeb.Services;

public class PatientRepository(LocalStorageService storage) : IPatientRepository
{
    private const string Key = "patients";

    private async Task<List<Patient>> LoadAsync() =>
        await storage.GetAsync<List<Patient>>(Key) ?? new List<Patient>();

    private async Task SaveAsync(List<Patient> list) =>
        await storage.SetAsync(Key, list);

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        var list = await LoadAsync();
        return list.Where(p => !p.IsDeleted);
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        var list = await LoadAsync();
        return list.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
    }

    public async Task AddAsync(Patient patient)
    {
        var list = await LoadAsync();
        patient.Id = list.Any() ? list.Max(p => p.Id) + 1 : 1;
        patient.CreatedAt = DateTime.Now;
        patient.IsDeleted = false;
        list.Add(patient);
        await SaveAsync(list);
    }

    public async Task UpdateAsync(Patient patient)
    {
        var list = await LoadAsync();
        var index = list.FindIndex(p => p.Id == patient.Id);
        if (index != -1)
        {
            patient.UpdatedAt = DateTime.Now;
            list[index] = patient;
            await SaveAsync(list);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var list = await LoadAsync();
        var patient = list.FirstOrDefault(p => p.Id == id);
        if (patient != null)
        {
            patient.IsDeleted = true;
            patient.UpdatedAt = DateTime.Now;
            await SaveAsync(list);
        }
    }
}
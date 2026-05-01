using SaluteWeb.Interfaces;
using SaluteWeb.Models;

namespace SaluteWeb.Services;

public class AnamnesisRepository(LocalStorageService storage) : IAnamnesisRepository
{
    private const string Key = "anamnesis";

    private async Task<List<Anamnesis>> LoadAsync() =>
        await storage.GetAsync<List<Anamnesis>>(Key) ?? new List<Anamnesis>();

    private async Task SaveAsync(List<Anamnesis> list) =>
        await storage.SetAsync(Key, list);

    public async Task<IEnumerable<Anamnesis>> GetAllAsync() =>
        (await LoadAsync()).Where(a => !a.IsDeleted);

    public async Task<Anamnesis?> GetByIdAsync(int id) =>
        (await LoadAsync()).FirstOrDefault(a => a.Id == id && !a.IsDeleted);

    public async Task<Anamnesis?> GetByPatientIdAsync(int patientId) =>
        (await LoadAsync()).FirstOrDefault(a => a.PatientId == patientId && !a.IsDeleted);

    public async Task AddAsync(Anamnesis anamnesis)
    {
        var list = await LoadAsync();
        anamnesis.Id = list.Any() ? list.Max(a => a.Id) + 1 : 1;
        anamnesis.CreatedAt = DateTime.Now;
        list.Add(anamnesis);
        await SaveAsync(list);
    }

    public async Task UpdateAsync(Anamnesis anamnesis)
    {
        var list = await LoadAsync();
        var index = list.FindIndex(a => a.Id == anamnesis.Id);
        if (index != -1)
        {
            anamnesis.UpdatedAt = DateTime.Now;
            list[index] = anamnesis;
            await SaveAsync(list);
        }
    }

    public async Task SaveOrUpdateAsync(Anamnesis anamnesis)
    {
        if (anamnesis.Id > 0)
            await UpdateAsync(anamnesis);
        else
            await AddAsync(anamnesis);
    }

    public async Task DeleteAsync(int id)
    {
        var list = await LoadAsync();
        var record = list.FirstOrDefault(a => a.Id == id);
        if (record != null)
        {
            record.IsDeleted = true;
            record.UpdatedAt = DateTime.Now;
            await SaveAsync(list);
        }
    }
}
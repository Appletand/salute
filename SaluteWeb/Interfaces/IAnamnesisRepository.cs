using SaluteWeb.Models;

namespace SaluteWeb.Interfaces;

public interface IAnamnesisRepository
{
    Task<IEnumerable<Anamnesis>> GetAllAsync();
    Task<Anamnesis?> GetByIdAsync(int id);
    Task<Anamnesis?> GetByPatientIdAsync(int patientId);
    Task AddAsync(Anamnesis anamnesis);
    Task UpdateAsync(Anamnesis anamnesis);
    Task SaveOrUpdateAsync(Anamnesis anamnesis);
    Task DeleteAsync(int id);
}
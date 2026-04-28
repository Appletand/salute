using Salute.Models;

namespace Salute.Interfaces;

public interface IAnamnesisRepository
{
    Task<Anamnesis?> GetByIdAsync(int id);
    Task<IEnumerable<Anamnesis>> GetByPatientIdAsync(int patientId);
    Task<Anamnesis?> GetLatestByPatientIdAsync(int patientId);
    Task AddAsync(Anamnesis anamnesis);
    Task UpdateAsync(Anamnesis anamnesis);
    Task DeleteAsync(int id);
}

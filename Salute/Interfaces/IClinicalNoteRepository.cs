using Salute.Models;

namespace Salute.Interfaces;

public interface IClinicalNoteRepository
{
    Task<IEnumerable<ClinicalNote>> GetByPatientIdAsync(int patientId);
    Task AddAsync(ClinicalNote note);
    Task UpdateAsync(ClinicalNote note);
    Task DeleteAsync(int id);
}

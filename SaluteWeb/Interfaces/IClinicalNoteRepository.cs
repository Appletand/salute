using SaluteWeb.Models;

namespace SaluteWeb.Interfaces;

public interface IClinicalNoteRepository
{
    Task<IEnumerable<ClinicalNote>> GetByPatientIdAsync(int patientId);
    Task AddAsync(ClinicalNote note);
    Task UpdateAsync(ClinicalNote note);
    Task DeleteAsync(int id);
}

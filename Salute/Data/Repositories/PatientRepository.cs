using Salute.Data;
using Salute.Interfaces;
using Salute.Models;
using Microsoft.EntityFrameworkCore;

namespace Salute.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;
    public PatientRepository(AppDbContext context) => _context = context;
    public async Task<Patient?> GetByIdAsync(int id) => await _context.Patients.FindAsync(id);
    public async Task<IEnumerable<Patient>> GetAllAsync() => await _context.Patients.ToListAsync();
    public async Task AddAsync(Patient patient) { _context.Patients.Add(patient); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Patient patient) { _context.Patients.Update(patient); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var p = await _context.Patients.FindAsync(id); if (p != null) { _context.Patients.Remove(p); await _context.SaveChangesAsync(); } }
}

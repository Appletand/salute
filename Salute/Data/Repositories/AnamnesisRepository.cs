using Salute.Data;
using Salute.Interfaces;
using Salute.Models;
using Microsoft.EntityFrameworkCore;

namespace Salute.Data.Repositories;

public class AnamnesisRepository : IAnamnesisRepository
{
    private readonly AppDbContext _context;
    public AnamnesisRepository(AppDbContext context) => _context = context;
    public async Task<Anamnesis?> GetByIdAsync(int id) => await _context.Anamneses.FindAsync(id);
    public async Task<IEnumerable<Anamnesis>> GetByPatientIdAsync(int patientId) => await _context.Anamneses.Where(a => a.PatientId == patientId).ToListAsync();
    public async Task<Anamnesis?> GetLatestByPatientIdAsync(int patientId) => await _context.Anamneses.Where(a => a.PatientId == patientId).OrderByDescending(a => a.Id).FirstOrDefaultAsync();
    public async Task AddAsync(Anamnesis anamnesis) { _context.Anamneses.Add(anamnesis); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Anamnesis anamnesis) { _context.Anamneses.Update(anamnesis); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id) { var a = await _context.Anamneses.FindAsync(id); if (a != null) { _context.Anamneses.Remove(a); await _context.SaveChangesAsync(); } }
}

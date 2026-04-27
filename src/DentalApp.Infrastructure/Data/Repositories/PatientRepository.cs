using Microsoft.EntityFrameworkCore;
using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using DentalApp.Infrastructure.Data.Context;

namespace DentalApp.Infrastructure.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;
    public PatientRepository(AppDbContext context) => _context = context;

    public async Task<Patient?> GetByIdAsync(int id) => await _context.Patients.FindAsync(id);
    public async Task<IEnumerable<Patient>> GetAllAsync() => await _context.Patients.ToListAsync();
    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var patient = await GetByIdAsync(id);
        if (patient != null) _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }
}
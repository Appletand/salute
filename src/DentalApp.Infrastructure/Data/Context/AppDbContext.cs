using Microsoft.EntityFrameworkCore;
using DentalApp.Domain.Entities;

namespace DentalApp.Infrastructure.Data.Context;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ClinicalNote> ClinicalNotes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Mapeamento para compatibilidade com Open Dental (exemplo)
        modelBuilder.Entity<Patient>().ToTable("patient");
        modelBuilder.Entity<Appointment>().ToTable("appointment");
        modelBuilder.Entity<ClinicalNote>().ToTable("clinicalnote");
    }
}
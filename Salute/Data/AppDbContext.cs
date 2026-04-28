using Microsoft.EntityFrameworkCore;
using Salute.Models;

namespace Salute.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Anamnesis> Anamneses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patients");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Anamnesis>(entity =>
        {
            entity.ToTable("Anamneses");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MainComplaint).IsRequired().HasMaxLength(500);
            // Adicione outras configurações conforme seu modelo
        });
    }
}
using DentalApp.Infrastructure.Data.Context;
using DentalApp.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class PatientRepositoryTests
{
    [Fact]
    public async Task AddPatient_ShouldPersist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        using var context = new AppDbContext(options);
        var repo = new PatientRepository(context);
        // Teste...
    }
}
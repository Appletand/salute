using DentalApp.Domain.Entities;
using Xunit;

namespace DentalApp.UnitTests.Patients;

public class PatientTests
{
    [Fact]
    public void Patient_ShouldHaveFirstName()
    {
        var patient = new Patient { FirstName = "Ana" };
        Assert.Equal("Ana", patient.FirstName);
    }
}
using System.Linq.Expressions;
using DentalApp.Domain.Entities;

namespace DentalApp.Domain.Specifications;

public static class PatientSpecifications
{
    public static Expression<Func<Patient, bool>> ByName(string searchTerm) =>
        p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm);
}
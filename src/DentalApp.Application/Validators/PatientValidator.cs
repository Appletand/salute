using FluentValidation;
using DentalApp.Domain.Entities;

namespace DentalApp.Application.Validators;

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Email).EmailAddress();
    }
}
using DentalApp.Application.Common.Interfaces;
using DentalApp.Domain.Entities;
using DentalApp.Shared.Helpers;

namespace DentalApp.Infrastructure.ExternalServices.MarkdownExport;

public class ObsidianExporter : IMarkdownExporter
{
    private readonly IPatientRepository _patientRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public ObsidianExporter(IPatientRepository patientRepo, IAppointmentRepository appointmentRepo)
    {
        _patientRepo = patientRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task ExportPatientAsync(Patient patient, string outputDirectory)
    {
        var dir = Path.Combine(outputDirectory, "Patients");
        Directory.CreateDirectory(dir);
        var filePath = Path.Combine(dir, $"{patient.LastName}_{patient.FirstName}.md");
        var content = $@"---
tags: [patient, dental]
name: {patient.FirstName} {patient.LastName}
dob: {patient.DateOfBirth:yyyy-MM-dd}
---

# {patient.FirstName} {patient.LastName}

**Telefone:** {patient.Phone}  
**Email:** {patient.Email}

## Anotações clínicas
(Placeholder para futuras notas)
";
        await File.WriteAllTextAsync(filePath, content);
    }

    public async Task ExportAppointmentAsync(Appointment appointment, string outputDirectory)
    {
        // Implementação similar
    }

    public async Task ExportAllToObsidianAsync(string vaultPath)
    {
        var patients = await _patientRepo.GetAllAsync();
        foreach (var patient in patients)
            await ExportPatientAsync(patient, vaultPath);
        // export appointments, etc.
    }
}
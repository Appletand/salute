using DentalApp.Domain.Entities;

namespace DentalApp.Application.Common.Interfaces;

public interface IMarkdownExporter
{
    Task ExportPatientAsync(Patient patient, string outputDirectory);
    Task ExportAppointmentAsync(Appointment appointment, string outputDirectory);
    Task ExportAllToObsidianAsync(string vaultPath);
}
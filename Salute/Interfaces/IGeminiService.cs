using System.Threading.Tasks;

namespace Salute.Interfaces;

public interface IGeminiService
{
    Task<string> GenerateClinicalSummaryAsync(string prompt);
    Task<string> GenerateAnamnesisAsync(string patientName, string? medicalHistory, string? mainSymptoms);
    Task<string> SuggestDiagnosisAsync(string anamnesisSummary, string? vitalSigns);
}
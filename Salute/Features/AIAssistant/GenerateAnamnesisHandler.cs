using Salute.Interfaces;

namespace Salute.Features.AIAssistant;

public record GenerateAnamnesisCommand(
    string PatientName,
    string? MedicalHistory = null,
    string? MainSymptoms = null
);

public record GenerateDiagnosisCommand(
    string AnamnesisSummary,
    string? VitalSigns = null
);

public record GenerateAnamnesisResponse(string SuggestedAnamnesis);
public record GenerateDiagnosisResponse(string Suggestions);

public class GenerateAnamnesisHandler
{
    private readonly IGeminiService _geminiService;

    public GenerateAnamnesisHandler(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    public async Task<GenerateAnamnesisResponse> Handle(GenerateAnamnesisCommand command)
    {
        var result = await _geminiService.GenerateAnamnesisAsync(
            command.PatientName,
            command.MedicalHistory,
            command.MainSymptoms);
        return new GenerateAnamnesisResponse(result);
    }

    public async Task<GenerateDiagnosisResponse> HandleDiagnosis(GenerateDiagnosisCommand command)
    {
        var result = await _geminiService.SuggestDiagnosisAsync(
            command.AnamnesisSummary,
            command.VitalSigns);
        return new GenerateDiagnosisResponse(result);
    }
}
using System.Threading.Tasks;
using Salute.Interfaces;

namespace Salute.Features.AIAssistant;

public record GenerateAnamnesisCommand(
    string PatientName,
    string? MedicalHistory = null,
    string? MainSymptoms = null
);

public record GenerateAnamnesisResponse(string SuggestedAnamnesis);

public class GenerateAnamnesisHandler
{
    private readonly IGeminiService _geminiService;
    
    public GenerateAnamnesisHandler(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }
    
    public async Task<GenerateAnamnesisResponse> Handle(GenerateAnamnesisCommand command)
    {
        var prompt = $@"
Você é um assistente odontológico especializado. Gere uma ANAMNESE ODONTOLÓGICA completa para o paciente:

Paciente: {command.PatientName}

{(string.IsNullOrEmpty(command.MedicalHistory) ? "" : $"Histórico médico: {command.MedicalHistory}")}
{(string.IsNullOrEmpty(command.MainSymptoms) ? "" : $"Sintomas principais: {command.MainSymptoms}")}

Formato: texto claro e profissional em português.
";
        var result = await _geminiService.GenerateClinicalSummaryAsync(prompt);
        return new GenerateAnamnesisResponse(result);
    }
}

using DentalApp.Application.Common.Interfaces;

namespace DentalApp.Application.Features.AIAssistant;

public record GenerateNoteCommand(string Symptoms, string PatientHistory);
public record GenerateNoteResponse(string GeneratedNote);

public class GenerateNoteHandler
{
    private readonly IGeminiService _gemini;
    public GenerateNoteHandler(IGeminiService gemini) => _gemini = gemini;
    public async Task<GenerateNoteResponse> Handle(GenerateNoteCommand command)
    {
        var prompt = $"Com base nos sintomas: {command.Symptoms} e histórico: {command.PatientHistory}, gere uma nota clínica odontológica.";
        var note = await _gemini.GenerateClinicalSummaryAsync(prompt);
        return new GenerateNoteResponse(note);
    }
}
using Salute.Interfaces;

namespace Salute.Services;

public class MockGeminiService : IGeminiService
{
    public Task<string> GenerateClinicalSummaryAsync(string prompt)
        => Task.FromResult("(Mock) Resumo clínico gerado.");

    public Task<string> GenerateAnamnesisAsync(string patientName, string? medicalHistory, string? mainSymptoms)
        => Task.FromResult($@"(Mock) Anamnese para {patientName}:
- Queixa Principal: {mainSymptoms ?? "Não informado"}
- Histórico: {medicalHistory ?? "Não informado"}
- Hipóteses: A investigar.");

    public Task<string> SuggestDiagnosisAsync(string anamnesisSummary, string? vitalSigns)
        => Task.FromResult("(Mock) Hipóteses diagnósticas: A investigar com base na anamnese.");
}
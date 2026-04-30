using System.Text;
using System.Text.Json;
using Salute.Interfaces;

namespace Salute.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    public GeminiService()
    {
        _httpClient = new HttpClient();
        _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
            ?? throw new InvalidOperationException("GEMINI_API_KEY não configurada no .env");
    }

    public async Task<string> GenerateClinicalSummaryAsync(string prompt)
    {
        return await SendPromptAsync(prompt);
    }

    public async Task<string> GenerateAnamnesisAsync(string patientName, string? medicalHistory, string? mainSymptoms)
    {
        var prompt = $@"
Você é um assistente clínico especializado. Gere uma ANAMNESE CLÍNICA completa e estruturada para o paciente abaixo.
Responda em português, de forma clara e profissional.

Paciente: {patientName}
{(string.IsNullOrEmpty(medicalHistory) ? "" : $"Histórico médico: {medicalHistory}")}
{(string.IsNullOrEmpty(mainSymptoms) ? "" : $"Sintomas principais: {mainSymptoms}")}

Estruture a resposta com as seções:
- Queixa Principal
- Histórico da Doença Atual
- Antecedentes Pessoais
- Hipóteses a investigar
- Recomendações iniciais
";
        return await SendPromptAsync(prompt);
    }

    public async Task<string> SuggestDiagnosisAsync(string anamnesisSummary, string? vitalSigns)
    {
        var prompt = $@"
Você é um assistente clínico especializado em apoio à decisão médica.
Com base na anamnese abaixo, sugira hipóteses diagnósticas e próximos passos.
Responda em português, de forma clara. Inclua um aviso de que sugestões são apenas auxiliares.

Anamnese:
{anamnesisSummary}

{(string.IsNullOrEmpty(vitalSigns) ? "" : $"Sinais vitais: {vitalSigns}")}

Estruture a resposta com:
- Hipóteses Diagnósticas (da mais à menos provável)
- Exames recomendados
- Conduta sugerida
- ⚠️ Aviso de que este é um apoio à decisão e não substitui avaliação médica
";
        return await SendPromptAsync(prompt);
    }

    private async Task<string> SendPromptAsync(string prompt)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BaseUrl}?key={_apiKey}", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Gemini API erro: {responseBody}");
            throw new HttpRequestException($"Gemini API retornou {response.StatusCode}");
        }

        using var doc = JsonDocument.Parse(responseBody);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text ?? "Sem resposta da IA.";
    }
}
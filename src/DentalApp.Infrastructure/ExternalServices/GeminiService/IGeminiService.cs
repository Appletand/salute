using System.Text;
using System.Text.Json;
using DentalApp.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DentalApp.Infrastructure.ExternalServices.GeminiService;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _endpoint;

    public GeminiService(HttpClient httpClient, IConfiguration config, IApiKeyProvider keyProvider)
    {
        _httpClient = httpClient;
        _apiKey = keyProvider.GetKey();
        _endpoint = config["Gemini:Endpoint"] ?? throw new Exception("Gemini endpoint missing");
    }

    public async Task<string> GenerateClinicalSummaryAsync(string prompt)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_endpoint}?key={_apiKey}", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);
        return doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? string.Empty;
    }
}
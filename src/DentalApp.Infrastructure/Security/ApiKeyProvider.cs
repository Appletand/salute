using Microsoft.Maui.Storage;
using DentalApp.Shared.Constants;

namespace DentalApp.Infrastructure.Security;

public interface IApiKeyProvider
{
    string GetKey();
    Task SetKeyAsync(string key);
}

public class ApiKeyProvider : IApiKeyProvider
{
    private string? _cachedKey;
    public string GetKey()
    {
        if (!string.IsNullOrEmpty(_cachedKey)) return _cachedKey;
        // Em ambiente real, usar SecureStorage - mas MAUI SecureStorage requer contexto.
        // Aqui colocamos um fallback para desenvolvimento (não usar em produção).
        _cachedKey = SecureStorage.GetAsync(AppConstants.GeminiApiKeyKey).GetAwaiter().GetResult()
                     ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                     ?? string.Empty;
        return _cachedKey;
    }

    public async Task SetKeyAsync(string key)
    {
        await SecureStorage.SetAsync(AppConstants.GeminiApiKeyKey, key);
        _cachedKey = key;
    }
}
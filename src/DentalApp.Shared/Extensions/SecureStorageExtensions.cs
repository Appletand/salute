using Microsoft.Maui.Storage;

namespace DentalApp.Shared.Extensions;

public static class SecureStorageExtensions
{
    public static async Task<string?> GetApiKeyAsync(string key)
    {
        try
        {
            return await SecureStorage.GetAsync(key);
        }
        catch
        {
            return null;
        }
    }

    public static async Task SetApiKeyAsync(string key, string value)
    {
        await SecureStorage.SetAsync(key, value);
    }
}
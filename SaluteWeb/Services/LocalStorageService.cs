using Microsoft.JSInterop;
using System.Text.Json;

namespace SaluteWeb.Services;

public class LocalStorageService(IJSRuntime js)
{
    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await js.InvokeVoidAsync("localStorage.setItem", key, json);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await js.InvokeAsync<string>("localStorage.getItem", key);
        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveAsync(string key) =>
        await js.InvokeVoidAsync("localStorage.removeItem", key);
}
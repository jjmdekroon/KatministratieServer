using Microsoft.JSInterop;
using System.Text.Json;

namespace Superkatten.Katministratie.Host.LocalStorage;

public class LocalStorageService : ILocalStorageService
{
    // See: https://developer.mozilla.org/en-US/docs/Web/API/Storage

    private readonly IJSRuntime _jsRuntime;

    public LocalStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

        if (json == null)
        {
            return default;
        }

        var result = JsonSerializer.Deserialize<T>(json);
        return result;
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", key, json);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
    }
}

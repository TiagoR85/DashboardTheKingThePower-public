using Blazored.LocalStorage;

namespace DashboardTheKingThePower.Services;

/// <summary>Wrapper fino do Blazored.LocalStorage para listas com seed.</summary>
public class StorageService<T> where T : class
{
    private readonly ILocalStorageService _localStorage;
    private readonly string _key;
    private readonly Func<List<T>> _seedFactory;

    public StorageService(ILocalStorageService localStorage, string key, Func<List<T>> seedFactory)
    {
        _localStorage = localStorage;
        _key = key;
        _seedFactory = seedFactory;
    }

    public async Task<List<T>> LoadAsync()
    {
        var items = await _localStorage.GetItemAsync<List<T>>(_key);
        if (items == null)
        {
            items = _seedFactory();
            await _localStorage.SetItemAsync(_key, items);
        }
        return items;
    }

    public async Task SaveAsync(List<T> items) => await _localStorage.SetItemAsync(_key, items);
}

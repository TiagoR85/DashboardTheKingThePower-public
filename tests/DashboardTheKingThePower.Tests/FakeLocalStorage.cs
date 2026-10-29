using Blazored.LocalStorage;

namespace DashboardTheKingThePower.Tests;

/// <summary>
/// Test double manual (sem biblioteca de mock) de ILocalStorageService (Blazored 4.5.0):
/// guarda em memória para testar StorageService/UserService sem JS.
/// </summary>
public sealed class FakeLocalStorage : ILocalStorageService
{
    public Dictionary<string, object> Store { get; } = new();

    public ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.TryGetValue(key, out var value) ? (T)value : default);

    public ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
    {
        Store[key] = data!;
        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
    {
        Store.Remove(key);
        return ValueTask.CompletedTask;
    }

    public ValueTask ClearAsync(CancellationToken cancellationToken = default)
    {
        Store.Clear();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> ContainsKeyAsync(string key, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.ContainsKey(key));

    public ValueTask<int> GetLengthAsync(CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.Count);

    public ValueTask<string> GetKeyAsync(int index, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.Keys.ElementAt(index));

    // --- membros ILocalStorageService 4.5.0 (assinaturas reais) ---
    public ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.TryGetValue(key, out var value) ? value as string : null);

    public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
    {
        Store[key] = data;
        return ValueTask.CompletedTask;
    }

    public ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.Keys.ElementAt(index));

    public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
        => ValueTask.FromResult<IEnumerable<string>>(Store.Keys.ToList());

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.ContainsKey(key));

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
        => ValueTask.FromResult(Store.Count);

    public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        foreach (var key in keys) Store.Remove(key);
        return ValueTask.CompletedTask;
    }

    public event EventHandler<ChangedEventArgs>? Changed
    {
        add { }
        remove { }
    }

    public event EventHandler<ChangingEventArgs>? Changing
    {
        add { }
        remove { }
    }
}

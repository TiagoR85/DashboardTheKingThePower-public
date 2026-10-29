using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DashboardTheKingThePower.Services;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly UserService _userService;
    private readonly NavigationManager _navigation;
    private const string UserKey = "current_user";
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public AuthStateProvider(
        ILocalStorageService localStorage,
        UserService userService,
        NavigationManager navigation)
    {
        _localStorage = localStorage;
        _userService = userService;
        _navigation = navigation;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var user = await GetStoredUserAsync();

            if (user == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.Id)
            }, "auth");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }

    public async Task LoginAsync(string email, string password)
    {
        try
        {
            var user = await _userService.ValidatePasswordAsync(email, password);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            var json = JsonSerializer.Serialize(user, JsonOpts);
            await _localStorage.SetItemAsStringAsync(UserKey, json);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        catch
        {
            throw;
        }
    }

    public async Task RegisterAsync(string name, string email, string password)
    {
        var user = await _userService.CreateUserAsync(name, email, password);
        var json = JsonSerializer.Serialize(user, JsonOpts);
        await _localStorage.SetItemAsStringAsync(UserKey, json);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(UserKey);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        _navigation.NavigateTo("login", new NavigationOptions { ForceLoad = false });
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        return await GetStoredUserAsync();
    }

    /// <summary>Atualiza nome/email do usuário logado (current_user + lista de usuários).</summary>
    public async Task UpdateProfileAsync(string newName, string newEmail)
    {
        var current = await GetStoredUserAsync();
        if (current == null) throw new InvalidOperationException("Nenhum usuário logado.");

        var updated = await _userService.UpdateProfileAsync(current.Id, newName.Trim(), newEmail.Trim());
        var json = JsonSerializer.Serialize(updated, JsonOpts);
        await _localStorage.SetItemAsStringAsync(UserKey, json);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<User?> GetStoredUserAsync()
    {
        try
        {
            var json = await _localStorage.GetItemAsStringAsync(UserKey);

            if (string.IsNullOrEmpty(json))
                return null;

            var user = JsonSerializer.Deserialize<User>(json, JsonOpts);
            return user;
        }
        catch
        {
            await _localStorage.RemoveItemAsync(UserKey);
            return null;
        }
    }
}

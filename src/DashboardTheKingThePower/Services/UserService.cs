using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using DashboardTheKingThePower.Models;

namespace DashboardTheKingThePower.Services;

public class UserService
{
    private readonly ILocalStorageService _localStorage;
    private const string StorageKey = "dashboard_users";
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public UserService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<User> CreateUserAsync(string name, string email, string password)
    {
        var users = await LoadUsersAsync();
        if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            throw new Exception("Email already exists");

        var salt = GenerateSalt();
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Email = email,
            PasswordHash = HashPassword(password, salt),
            Salt = salt,
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        users.Add(user);
        await SaveUsersAsync(users);
        return user;
    }

    public async Task<User?> ValidatePasswordAsync(string email, string password)
    {
        var users = await LoadUsersAsync();
        var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (user == null) return null;

        var hash = HashPassword(password, user.Salt);
        return hash == user.PasswordHash ? user : null;
    }

    public async Task<bool> UpdatePasswordAsync(string email, string oldPassword, string newPassword)
    {
        var user = await ValidatePasswordAsync(email, oldPassword);
        if (user == null) return false;

        var users = await LoadUsersAsync();
        var stored = users.First(u => u.Id == user.Id);
        stored.Salt = GenerateSalt();
        stored.PasswordHash = HashPassword(newPassword, stored.Salt);
        await SaveUsersAsync(users);
        return true;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var users = await LoadUsersAsync();
        return users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Atualiza nome/email de um usuário na lista persistida.</summary>
    public async Task<User> UpdateProfileAsync(string userId, string newName, string newEmail)
    {
        var users = await LoadUsersAsync();
        var user = users.FirstOrDefault(u => u.Id == userId)
            ?? throw new InvalidOperationException("Usuário não encontrado.");
        user.Name = newName;
        user.Email = newEmail;
        await SaveUsersAsync(users);
        return user;
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combined = $"{salt}{password}{salt}";
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(bytes);
    }

    private static string GenerateSalt()
    {
        var saltBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private async Task<List<User>> LoadUsersAsync()
    {
        try
        {
            var json = await _localStorage.GetItemAsStringAsync(StorageKey);

            if (string.IsNullOrEmpty(json))
                return new List<User>();

            var users = JsonSerializer.Deserialize<List<User>>(json, JsonOpts);
            return users ?? new List<User>();
        }
        catch
        {
            await _localStorage.RemoveItemAsync(StorageKey);
            return new List<User>();
        }
    }

    private async Task SaveUsersAsync(List<User> users)
    {
        try
        {
            var json = JsonSerializer.Serialize(users, JsonOpts);
            await _localStorage.SetItemAsStringAsync(StorageKey, json);
        }
        catch
        {
            throw;
        }
    }
}

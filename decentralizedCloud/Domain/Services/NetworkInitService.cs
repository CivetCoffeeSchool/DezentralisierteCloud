
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using Domain.Repositories.DTOs;
using Domain.Repositories.Interfaces;
using Model.Entities;

namespace Domain.Services;

    // ManagementServer/Services/NetworkInitService.cs
public class NetworkInitService
{
    private const string ConfigPath = "/app/config/network-config.json";
    private readonly IUserRepository _userRepo;
    private readonly BcryptPasswordHasher _hasher;

    public NetworkInitService(
        IUserRepository userRepo, 
        BcryptPasswordHasher hasher)
    {
        _userRepo = userRepo;
        _hasher = hasher;
    }

    public async Task InitializeNetworkAsync()
    {
        if (File.Exists(ConfigPath)) return;

        var config = new NetworkConfig {
            NetworkId = Guid.NewGuid().ToString("N"),
            NetworkKey = GenerateCryptoKey(),
            CreatedAt = DateTime.UtcNow
        };

        // Create default admin user
        var hash = _hasher.CreateHash("admin123");
        await _userRepo.CreateAsync(new AdminUser() {
            Username = "admin",
            PasswordHash = hash,
        });

        await File.WriteAllTextAsync(ConfigPath, 
            JsonSerializer.Serialize(config, new JsonSerializerOptions {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
    }

    private static string GenerateCryptoKey()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32]; // 256-bit key
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

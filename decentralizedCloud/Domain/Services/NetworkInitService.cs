using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.Entities;

namespace Domain.Services;

public class NetworkInitService
{
    // ManagementServer/Services/NetworkInitService.cs
    public class NetworkInitService
    {
        private const string ConfigPath = "/app/config/network-config.json";
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher<> _hasher;

        public NetworkInitService(
            IUserRepository userRepo, 
            IPasswordHasher hasher)
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
            var (hash, salt) = _hasher.CreateHash("admin123");
            await _userRepo.CreateAsync(new User {
                Username = "admin",
                PasswordHash = hash,
                PasswordSalt = salt,
                IsAdmin = true
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
}
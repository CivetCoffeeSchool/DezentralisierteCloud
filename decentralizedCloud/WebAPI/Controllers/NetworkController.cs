using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace WebAPI.Controllers;

public class NetworkController:Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IDataOwnershipRepository _dataOwnershipRepository;
    [HttpPost]
    public async Task<IActionResult> StartNetwork(string adminUsername, string adminPassword)
    {
        // Create admin user
        var salt = GenerateSalt();
        var passwordHash = ComputeHash(adminPassword, salt);

        var admin = new User
        {
            Username = adminUsername,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(salt),
            IsAdmin = true
        };

        await _userRepository.CreateAsync(admin);

        // Create super peer
        var superPeer = new Peer
        {
            Credential = Guid.NewGuid().ToString(),
            IsSuperpeer = true
        };

        _context.Peers.Add(superPeer);
        _context.SaveChanges();

        return Ok(new { Message = "Network started", Credential = superPeer.Credential });
    }

    [HttpPost]
    public IActionResult JoinNetwork(string credential)
    {
        var peer = _context.Peers.FirstOrDefault(p => p.Credential == credential);

        if (peer == null)
            return Unauthorized("Invalid credential");

        return Ok(new { Message = "Peer joined successfully" });
    }

    private byte[] GenerateSalt()
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);
        return salt;
    }

    private string ComputeHash(string password, byte[] salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password).Concat(salt).ToArray()));
    }
}
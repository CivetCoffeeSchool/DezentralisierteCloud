using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/network")]
public class NetworkController:ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IDataRepository _dataOwnershipRepository;
    private readonly IPeerRepository _peerRepository;
    private NetworkConfiguration _networkConfiguration;

    [HttpPost("start")]
    public async Task<IActionResult> StartNetwork(string adminUsername, string adminPassword, int totalSpace)
    {
        _networkConfiguration = new NetworkConfiguration();
        
        // Create admin user
        var salt = GenerateSalt();
        var passwordHash = ComputeHash(adminPassword, salt);

        var admin = new User
        {
            Username = adminUsername,
            PasswordHash = passwordHash,
            PasswordSalt = Convert.ToBase64String(salt),
            UserType = "USER"
        };

        await _userRepository.CreateAsync(admin);
        
        
        // Create super peer
        var superPeer = new Peer
        {
            peerType = "PEER",
            TotalSpace = totalSpace,
            IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
            Port = HttpContext.Connection.RemotePort
        };
        
        await _peerRepository.CreateAsync(superPeer);

        return Ok(new { Message = "Network started"});
    }

    [HttpPost("join/{credential}")]
    public async Task<IActionResult> JoinNetwork(string credential, [FromBody] int totalSpace)
    { 
        if (credential != _networkConfiguration.NetworkHash)
            return Unauthorized();
        await _peerRepository.CreateAsync(new Peer
        {
            peerType = "PEER",
            TotalSpace = totalSpace,
            IpAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
            Port = HttpContext.Connection.RemotePort
        });

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
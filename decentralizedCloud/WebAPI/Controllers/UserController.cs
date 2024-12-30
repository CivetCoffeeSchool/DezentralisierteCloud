using Domain.Repositories.Implementations;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using WebAPI.DTOs;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/users")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            return BadRequest("Username already exists.");

        var (hash, salt) = GenerateHashAndSalt(dto.Password);
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsAdmin = false
        };

        await _userRepository.CreateAsync(user);
        return Ok("Registration successful.");
    }
    
    private static (string hash, string salt) GenerateHashAndSalt(string password)
    {
        var salt = Guid.NewGuid().ToString();
        var hash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(password + salt)));
        return (hash, salt);
    }

    private static bool VerifyPassword(string password, string hash, string salt)
    {
        var computedHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(password + salt)));
        return computedHash == hash;
    }
}

using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AuthController:ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _authService.AuthenticateAsync(dto.Username, dto.Password);
        return user != null 
            ? Ok(GenerateJwt(user)) 
            : Unauthorized();
    }
}
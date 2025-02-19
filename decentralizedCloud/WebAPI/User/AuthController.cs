using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AuthController:ControllerBase
{
    private readonly AuthService _authService;
    private readonly JwtService _jwtService;

    public AuthController(AuthService authService, JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _authService.AuthenticateAsync(dto.Username, dto.Password);
        return user != null 
            ? Ok(_jwtService.GenerateJwt(user)) 
            : Unauthorized();
    }
}
//TODO: JWT entfohlen nicht zu verwenden, sondern jedes mal einlogen 
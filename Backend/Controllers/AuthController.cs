using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Backend.Services;
using Backend.Models;
using Backend.Interfaces;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;

    public AuthController(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var user = await _userService.ValidateUserAsync(login.Email, login.Password);
        if (user == null) return Unauthorized("Credenciales incorrectas");

        var token = GenerateToken(user);
        return Ok(new {
        token = token,
        user = new {
            user.Id,
            user.Name,
            user.LastName,
            user.Email,
            user.Rol
        }
    });
    }

    private string GenerateToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim(ClaimTypes.Role, user.Rol) // si usas roles
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record LoginDto(string Email, string Password);

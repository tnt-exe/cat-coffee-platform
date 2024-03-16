using BusinessObject.Enums;
using DTO.UserDTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatCoffeePlatformAPI.Service;

public interface IJWTTokenService
{
    string CreateToken(UserResponseDTO user);
}

public class JWTTokenService : IJWTTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IKeyManager _keyManager;

    public JWTTokenService(IConfiguration configuration, IKeyManager keyManager)
    {
        _configuration = configuration;
        _keyManager = keyManager;
    }

    public string CreateToken(UserResponseDTO user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? "Undefined"),
            new Claim("Fisrt Name", user.FirstName ?? "Undefined"),
            new Claim("Last Name", user.LastName ?? "Undefined"),
            new Claim(ClaimTypes.Email, user.Email ?? "Undefined" ),
            new Claim("scope", user.Role.ToString() ?? Role.Undefined.ToString())
        };

        var key = new RsaSecurityKey(_keyManager.RsaKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}

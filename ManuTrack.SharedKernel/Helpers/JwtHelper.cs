using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManuTrack.SharedKernel.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(
        int userId, string email, string role, string name,
        string secretKey, string issuer, string audience,
        int expiryMinutes = 60)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static int GetUserId(ClaimsPrincipal principal)
    {
        var value = principal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier
                               || c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        return int.TryParse(value, out var id) ? id : 0;
    }

    public static string? GetRole(ClaimsPrincipal principal) =>
        principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

    public static string? GetName(ClaimsPrincipal principal) =>
        principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

    public static string? GetEmail(ClaimsPrincipal principal) =>
        principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email
                                           || c.Type == JwtRegisteredClaimNames.Email)?.Value;
}
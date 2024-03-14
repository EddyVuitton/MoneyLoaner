using Microsoft.IdentityModel.Tokens;
using MoneyLoaner.Data.Auth;
using MoneyLoaner.WebAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MoneyLoaner.WebAPI.Helpers;

public static class AuthHelper
{
    public static string HashPassword(string password)
    {
        var passwordBytes = Encoding.Default.GetBytes(password ?? string.Empty);
        var hashedPassword = SHA256.HashData(passwordBytes);

        return Convert.ToHexString(hashedPassword);
    }

    public static UserToken BuildToken(string email, int clientId, SymmetricSecurityKey symmetricSecurityKey)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, email),
            new("ClientId", clientId.ToString())
        };

        var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.Now.AddYears(15);

        JwtSecurityToken token = new(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
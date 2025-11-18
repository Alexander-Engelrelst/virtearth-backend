using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adria.Application.Authentication;
using Adria.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Adria.Infrastructure;

public sealed class JwtProvider : IJwtProvider
{
    public string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new Claim("Guid", user.Id.ToString()), // Subject is standardized for unique identifiers
        ];
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.Secret));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: JwtConfiguration.Issuer,
            audience: JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(JwtConfiguration.ExpireDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public abstract record JwtConfiguration
{
    public static string Secret = "L7dLr6B6K9M+f0Ogbbuv9y8RnWUVVYqJ7Zn1jZy4WGi8sVtzjQw1v5XvT4Qy2x+O9U9JvUdxW1BvJQnHVpEtDw==";
    public static string Issuer = "VirtEarth server";
    public static string Audience = "VirtEarth player";
    public static int ExpireDays = 7;
}
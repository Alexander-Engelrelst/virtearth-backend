using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adria.Application.Authentication;
using Adria.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Adria.Infrastructure;

public class JwtProvider : IJwtProvider
{
    private static readonly JwtConfiguration _jwtConfiguration = new JwtConfiguration();
    public string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject is standardized for unique identifiers
            new Claim(JwtRegisteredClaimNames.PreferredUsername, user.Username)
        ];
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtConfiguration.ExpireDays),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public record JwtConfiguration
{
    public string Secret { get; set; } = "L7dLr6B6K9M+f0Ogbbuv9y8RnWUVVYqJ7Zn1jZy4WGi8sVtzjQw1v5XvT4Qy2x+O9U9JvUdxW1BvJQnHVpEtDw==";
    public string Issuer { get; set; } = "VirtEarth server";
    public string Audience { get; set; } = "VirtEarth player";
    public int ExpireDays { get; set; } = 7;
}
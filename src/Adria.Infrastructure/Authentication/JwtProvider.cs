using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adria.Application.Contracts;
using Adria.Domain.Users;
using Microsoft.IdentityModel.Tokens;

namespace Adria.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    public string GenerateToken(User user)
    {
        List<Claim> claims =
        [
            new Claim("guid", user.Id.ToString())
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

public static class JwtConfiguration
{
    // Thanks Sonar
    private const string SECRET =
        "L7dLr6B6K9M+f0Ogbbuv9y8RnWUVVYqJ7Zn1jZy4WGi8sVtzjQw1v5XvT4Qy2x+O9U9JvUdxW1BvJQnHVpEtDw==";

    private const string ISSUER = "VirtEarth server";
    private const string AUDIENCE = "VirtEarth player";
    private const int EXPIRE_DAYS = 7;

    public static string Secret => SECRET;
    public static string Issuer => ISSUER;
    public static string Audience => AUDIENCE;
    public static int ExpireDays => EXPIRE_DAYS;
    
}
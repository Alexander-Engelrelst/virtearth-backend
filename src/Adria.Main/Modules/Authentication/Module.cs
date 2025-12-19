using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Adria.Application.Contracts;
using Adria.Infrastructure;
using Adria.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Adria.Main.Modules.Authentication;

public static class Module
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization();
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtConfiguration.Issuer,
                    ValidAudience = JwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.Secret)),
                    RequireExpirationTime = true
                };
            });
    }
}

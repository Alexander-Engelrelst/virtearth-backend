using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Adria.Application.Authentication;
using Adria.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Adria.Main.Modules.Authentication;

public static class Module
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization();
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        
        return services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtConfiguration.Issuer,
                    ValidAudience = JwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfiguration.Secret)),
                    RequireExpirationTime = true,
                };

                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var idClaim = context.Principal?.FindFirst("Guid")?.Value;

                        if (idClaim is null)
                        {
                            context.Fail("Missing id in token");
                            return Task.CompletedTask;
                        }

                        Guid id = new Guid(idClaim);
                        
                        if (!context.HttpContext.Request.RouteValues.TryGetValue("id", out object? routeIdObj))
                        {
                            Console.WriteLine("issue");
                            context.Fail("Missing route ID");
                            return Task.CompletedTask;
                        }
                        Console.WriteLine(routeIdObj);
                        if (routeIdObj is null) throw new ArgumentNullException(nameof(routeIdObj));
                        Guid routeId = Guid.Parse(routeIdObj.ToString()!);

                        if (routeId == id)
                        {
                            context.Success();
                        }
                        else
                        {
                            context.Fail("UserId in route and in token are not the same");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Adria.Application.Authentication;
using Adria.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Adria.Main.Modules.Authentication;
// me: ╰༼=ಠਊಠ=༽╯ sonar: ( ° ͜ʖ͡°)╭∩╮
#pragma warning disable

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
                        
                        if (!context.HttpContext.Request.Query.TryGetValue("id", out StringValues idFromQuery))
                        {
                            context.Fail("Missing route ID");
                            return Task.CompletedTask;
                        }

                        if (idFromQuery.Count != 1)
                        {
                            throw new ArgumentOutOfRangeException(nameof(idFromQuery), "please enter exactly one id");
                        }
                        if (!Guid.TryParse(idFromQuery[0], out Guid userGivenId))
                        {
                            context.Fail("Invalid route ID");
                            return Task.CompletedTask;
                        }
                        

                        if (userGivenId == id)
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
#pragma warning restore
using Adria.Application.Authentication;
using Adria.Infrastructure;

namespace Adria.Main.Modules.Authentication;

public static class Module
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        return services.AddScoped<IJwtProvider, JwtProvider>();
    }
}
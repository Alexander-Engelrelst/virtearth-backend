using Adria.Application.Authentication;
using Adria.Infrastructure;

namespace Adria.Main.Modules.Authentication;

public static class Module
{
    private static IServiceCollection AddAdoServices(
        this IServiceCollection services
    )
    {
        return services.AddScoped<IJwtProvider, JwtProvider>();
    }
}
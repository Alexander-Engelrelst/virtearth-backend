using Adria.Application.Contracts;
using Adria.Application.Users;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services.AddScoped<IUseCase<CheckUsernameInUseInput, Task<bool>>, CheckUsernameInUse>();
    }
}

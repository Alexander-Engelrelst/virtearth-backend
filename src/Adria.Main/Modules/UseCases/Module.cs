using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Users;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<CheckUsernameInUseInput, Task<bool>>, CheckUsernameInUse>()
            .AddScoped<IUseCase<CreateUserInput, Task<CreateUserResult>>, CreateUser>()
            .AddScoped<IUseCase<Guid, Task<string>>, Login>();
    }
}

using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.games;
using Adria.Application.Users;
using Adria.Domain.games;
using Adria.Domain.Users;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<CheckUsernameInUseInput, Task<bool>>, CheckUsernameInUse>()
            .AddScoped<IUseCase<CreateUserInput, Task<UserData>>, CreateUser>()
            .AddScoped<IUseCase<Guid, Task<UserData>>, Login>()
            .AddScoped<IUseCase<ChangeUserNameInput, Task<UserData>>, ChangeUserName>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<GameLocation>>>, GetGamesLocations>()
            .AddScoped<IUseCase<Guid, Task<User>>, GetUser>()
            .AddScoped<IUseCase<StartGameInput, Task<Game>>, StartGame>();
    }
}

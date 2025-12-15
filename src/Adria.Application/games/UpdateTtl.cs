using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Users;

namespace Adria.Application.games;

public sealed record UpdateTtlInput(User User, Guid GameId);

public class UpdateTtl : IUseCase<UpdateTtlInput>
{
    public Task Execute(UpdateTtlInput input)
    {
        Game game = ActiveGames.Get(input.User.Id, input.GameId);
        game.UpdateTtl();
        return Task.CompletedTask;
    }
}
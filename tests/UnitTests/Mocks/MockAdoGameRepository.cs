using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;

namespace UnitTests.Mocks;

public class MockAdoGameRepository : IGameRepository
{
    public readonly IList<Game> CompletedGames = [
        new MazeGame(Guid.NewGuid(), new User("username"), MockHelpers.GenerateMockArtifacts(5))
    ];
    public Task Save(Game game)
    {
        if (CompletedGames.Contains(game))
        {
            throw new GameAlreadyCompletedByUserException(game.GameId, game.User.Id);
        }
        
        CompletedGames.Add(game);
        return Task.CompletedTask;
    }
}
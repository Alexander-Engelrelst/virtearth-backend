using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class SaveFinishedGameTests
{
    [Fact]
    public async Task SavingNonExistentGameThrows()
    {
        var usecase = new SaveFinishedGame(new NullLogger<SaveFinishedGame>(), new MockAdoGameRepository());
        await Assert.ThrowsAsync<ActiveGameNotFoundException>(() =>
            usecase.Execute(new SaveFinishedGameInput(new User("username"), Guid.NewGuid())
            ));
    }

    [Fact]
    public async Task SavingUnfinishedGameThrows()
    {
        var usecase = new SaveFinishedGame(new NullLogger<SaveFinishedGame>(), new MockAdoGameRepository());
        Game game = new MazeGame(Guid.NewGuid(), new User("username", Guid.NewGuid()), MockHelpers.GenerateMockArtifacts(5));
        ActiveGames.AddGame(game);
        
        await Assert.ThrowsAsync<GameNotFinishedException>(() => usecase.Execute(
            new SaveFinishedGameInput(game.User, game.GameId)
            ));
    }
}
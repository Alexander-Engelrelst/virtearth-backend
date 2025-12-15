using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class StartGameTests
{
    [Fact]
    public async Task StartingGameWithoutArtifactsThrows()
    {
        var mockQuery = new MockArtifactsQuery();
        var usecase = new StartGame(new NullLogger<StartGame>(), mockQuery);
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            usecase.Execute(new StartGameInput(mockQuery.GameWithoutArtifactsId, new User("thisisavalidusername")))
        );
        Assert.Equal("artifacts",  exception.ParamName);
    }
    
    [Fact]
    public async Task StartingGameWithArtifactsWorks()
    {
        User user = new("thisisaveryvalidusername");
        var usecase = new StartGame(new NullLogger<StartGame>(), new MockArtifactsQuery());
        Game game = await usecase.Execute(new StartGameInput(Guid.NewGuid(), user));
        
        // this checks if the game has successfully been added to the active games
        Assert.Equal(game, ActiveGames.Get(game.User.Id));
    }

    [Fact]
    public async Task StartingGameForUserAlreadyPlayingThrows()
    {
        User user = new("thisisaveryvalidusername");
        var usecase = new StartGame(new NullLogger<StartGame>(), new MockArtifactsQuery());
        await usecase.Execute(new StartGameInput(Guid.NewGuid(), user));
        await Assert.ThrowsAsync<PlayerAlreadyPlayingException>(
            () => usecase.Execute(new StartGameInput(Guid.NewGuid(), user))
        );
    }
}
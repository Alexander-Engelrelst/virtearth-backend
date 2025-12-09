using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
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
        Assert.True(ActiveGames.Games.ContainsKey(user.Id));
        Assert.Equal(game, ActiveGames.Games[user.Id]);
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
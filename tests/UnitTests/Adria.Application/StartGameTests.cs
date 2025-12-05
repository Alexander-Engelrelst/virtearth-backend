using Adria.Application.games;
using Adria.Domain.games;
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
            usecase.Execute(new StartGameInput(mockQuery.GameWithoutArtifactsId, Guid.NewGuid()))
        );
        Assert.Equal("artifacts",  exception.ParamName);
    }
    
    [Fact]
    public async Task StartingGameWithArtifactsWorks()
    {
        var usecase = new StartGame(new NullLogger<StartGame>(), new MockArtifactsQuery());
        Game game = await usecase.Execute(new StartGameInput(Guid.NewGuid(), Guid.NewGuid()));
        Assert.Single(ActiveGames.Games);
        Assert.Equal(game, ActiveGames.Games[0]);
    }
}
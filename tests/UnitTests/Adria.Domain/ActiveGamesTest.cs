using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using UnitTests.Mocks;
using Xunit.Abstractions;

namespace UnitTests.Adria.Domain;

public class ActiveGamesTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ActiveGamesTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void AddingCorrectGameWorks()
    {
        Game game = new MazeGame(Guid.NewGuid(), new User("username"), MockHelpers.GenerateMockArtifacts(5));
        ActiveGames.AddGame(game);
        Assert.Equal(game, ActiveGames.Get(game.User.Id));
    }

    [Fact]
    public void AddingGameTwiceForTheSameUserThrows()
    {
        Game game =  new MazeGame(Guid.NewGuid(), new User("username"), MockHelpers.GenerateMockArtifacts(5));
        ActiveGames.AddGame(game);
        
        Assert.Throws<PlayerAlreadyPlayingException>(() => ActiveGames.AddGame(game));
    }

    [Fact]
    public void GettingNonExistingGameThrows()
    {
        Assert.Throws<ActiveGameNotFoundException>(() => ActiveGames.Get(Guid.NewGuid()));
    }
    
}
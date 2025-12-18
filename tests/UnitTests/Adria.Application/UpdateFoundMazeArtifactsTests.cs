using Adria.Application.games;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging.Abstractions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class UpdateFoundMazeArtifactsTests
{
    [Fact]
    public async Task UpdatingNonExistingGameThrows()
    {
        var usecase = new UpdateFoundMazeArtifacts(new NullLogger<UpdateFoundMazeArtifacts>());
        await Assert.ThrowsAsync<ActiveGameNotFoundException>(() => usecase.Execute(new UpdateFoundMazeArtifactsInput(
            new User("username"),
            Guid.NewGuid(),
            Guid.NewGuid(),
            1,
            1,
            90
        )));
    }

    [Fact]
    public async Task UpdatingWithWrongGameIdThrows()
    {
        var usecase = new UpdateFoundMazeArtifacts(new NullLogger<UpdateFoundMazeArtifacts>());
        Game game = new MazeGame(Guid.NewGuid(), new User("username"), MockHelpers.GenerateMockArtifacts(10));
        ActiveGames.AddGame(game);

        await Assert.ThrowsAsync<GameIdMismatchException>(() =>
            usecase.Execute(new UpdateFoundMazeArtifactsInput(game.User, Guid.NewGuid(), Guid.NewGuid(), 1, 1, 90))
        );
    }

    [Fact]
    public async Task UpdatingNonExistingArtifactThrows()
    {
        var usecase = new UpdateFoundMazeArtifacts(new NullLogger<UpdateFoundMazeArtifacts>());
        Game game =  new MazeGame(Guid.NewGuid(), new User("username"), MockHelpers.GenerateMockArtifacts(10));
        ActiveGames.AddGame(game);

        await Assert.ThrowsAsync<ArtifactNotFoundException>(() =>
            usecase.Execute(new UpdateFoundMazeArtifactsInput(game.User, Guid.NewGuid(), game.GameId, 1, 1, 90)));
    }
    
    [Fact]
    public async Task UpdatingAlreadyFoundArtifactThrows()
    {
        var usecase = new UpdateFoundMazeArtifacts(new NullLogger<UpdateFoundMazeArtifacts>());
        var artifacts = MockHelpers.GenerateMockArtifacts(10);
        MazeArtifact artifact = new MazeArtifact(Guid.NewGuid(), "name", "description");

        artifacts.Add(artifact);

        Game game = new MazeGame(Guid.NewGuid(), new User("username"), artifacts);
        
        ActiveGames.AddGame(game);
        await usecase.Execute(new UpdateFoundMazeArtifactsInput(game.User, artifact.Id, game.GameId, 1, 1, 90));
        await Assert.ThrowsAsync<ArtifactAlreadyFoundException>(() =>
            usecase.Execute(new UpdateFoundMazeArtifactsInput(game.User, artifact.Id, game.GameId, 1, 1, 90)));
    }

    [Fact]
    public async Task CorrectUsageWorks()
    {
        var usecase = new UpdateFoundMazeArtifacts(new NullLogger<UpdateFoundMazeArtifacts>());
        MazeArtifact artifact = new(Guid.NewGuid(), "name", "description");
        MazeArtifact artifact2 = new(Guid.NewGuid(), "name", "description");
        Game game = new MazeGame(Guid.NewGuid(), new User("username"), new HashSet<MazeArtifact>{artifact, artifact2});

        ActiveGames.AddGame(game);
        
        Assert.Null(await usecase.Execute(new UpdateFoundMazeArtifactsInput(game.User, artifact.Id, game.GameId, 1, 1, 90)));
        Assert.Equal(game, await usecase.Execute(new UpdateFoundMazeArtifactsInput(game.User, artifact2.Id, game.GameId, 1, 1, 90)));
    }
    
    /* if the value returned is actually correct will be checked inside the functions called by the usecase
     * the exceptions thrown are checked since these are very important to ensure catching in the controller*/
}
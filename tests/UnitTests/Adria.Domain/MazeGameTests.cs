using System.Collections.Immutable;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using UnitTests.Mocks;
using Xunit.Abstractions;

namespace UnitTests.Adria.Domain;

public class MazeGameTest
{
    [Fact]
    public void MazeGameWithEmptyGameIdThrows()
    {
        Assert.Throws<ArgumentException>(() =>
            new MazeGame(Guid.Empty, new User("username"), MockHelpers.GenerateMockArtifacts(5))
        );
    }
    
    [Fact]
    public void MazeGameWithoutArtifactsThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new MazeGame(Guid.NewGuid(), new User("username"), MockHelpers.GenerateMockArtifacts(0))
        );
    }

    [Fact]
    public void MazeGameWithArtifactsDoesNotThrow()
    {
        var artifacts = MockHelpers.GenerateMockArtifacts(5);
        MazeGame game = new MazeGame(Guid.NewGuid(), new User("username"), artifacts);
        foreach (var artifact in artifacts)
        {
            Assert.Contains(artifact, game.Artifacts);
        }
    }

    [Fact]
    public void UpdateMazeGameFoundArtifactTest()
    {
        var artifacts = MockHelpers.GenerateMockArtifacts(5);
        var artifact = new MazeArtifact(Guid.NewGuid(), "name", "description");
        artifacts.Add(artifact);
        
        MazeGame game = new MazeGame(Guid.NewGuid(), new User("username"), artifacts);
        // location 0, 0 can never be a valid location 
        game.UpdateUserFoundArtifacts(artifact.Id, 1 , 1 , 90);
        Assert.Contains(artifact, game.FoundArtifacts);
        
        Assert.Throws<ArtifactAlreadyFoundException>(() => game.UpdateUserFoundArtifacts(artifact.Id, 1, 1, 90));
        Assert.Throws<ArtifactNotFoundException>(() => game.UpdateUserFoundArtifacts(Guid.NewGuid(), 1, 1, 90));
    }
    // TODO add tests to check if an exit actually gets generated
}
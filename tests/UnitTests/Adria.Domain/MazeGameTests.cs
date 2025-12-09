using System.Collections.Immutable;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Xunit.Abstractions;

namespace UnitTests.Adria.Domain;

public class MazeGameTest
{
    [Fact]
    public void MazeGameWithEmptyGameIdThrows()
    {
        Assert.Throws<ArgumentException>(() => new MazeGame(Guid.Empty, new User("username"), GetMockArtifacts(5)));
    }
    
    [Fact]
    public void MazeGameWithoutArtifactsThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MazeGame(Guid.NewGuid(), new User("username"), GetMockArtifacts(0)));
    }

    [Fact]
    public void MazeGameWithArtifactsDoesNotThrow()
    {
        var artifacts = GetMockArtifacts(5);
        MazeGame game = new MazeGame(Guid.NewGuid(), new User("username"), artifacts);
        foreach (var artifact in artifacts)
        {
            Assert.Contains(artifact, game.Artifacts);
        }
    }

    [Fact]
    public void UpdateMazeGameFoundArtifactTest()
    {
        var artifacts = GetMockArtifacts(5);
        var artifact = new MazeArtifact(Guid.NewGuid(), "name", "description");
        artifacts.Add(artifact);
        
        MazeGame game = new MazeGame(Guid.NewGuid(), new User("username"), artifacts);
        game.UpdateUserFoundArtifacts(artifact.Id);
        Assert.Contains(artifact, game.FoundArtifacts);
        
        Assert.Throws<ArtifactAlreadyFoundException>(() => game.UpdateUserFoundArtifacts(artifact.Id));
        Assert.Throws<ArtifactNotFoundException>(() => game.UpdateUserFoundArtifacts(Guid.NewGuid()));
    }
    

    // TODO here will be more tests added in further issues
    private static HashSet<MazeArtifact> GetMockArtifacts(int numberOfArtifacts)
    {
        HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();
        for (int i = 0; i < numberOfArtifacts; i++)
        {
            artifacts.Add(new MazeArtifact(Guid.NewGuid(), $"artifact{i}", $"description{i}"));
        }

        return artifacts;
    }
}
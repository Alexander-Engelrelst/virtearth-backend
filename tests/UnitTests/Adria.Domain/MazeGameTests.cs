using Adria.Domain.games;
using Xunit.Abstractions;

namespace UnitTests.Adria.Domain;

public class MazeGameTest
{
    // TODO write actual tests lol (maybe a flood algorithm to check if every artifacts is reachable from any starting point in the maze
    [Fact]
    public void MazeGameWithEmptyGameIdThrows()
    {
        Assert.Throws<ArgumentException>(() => new MazeGame(Guid.Empty, Guid.NewGuid(), GetMockArtifacts(5)));
    }
    
    [Fact]
    public void MazeGameWithEmptyUserIdThrows()
    {
        Assert.Throws<ArgumentException>(() => new MazeGame(Guid.NewGuid(), Guid.Empty, GetMockArtifacts(5)));
    }
    
    [Fact]
    public void MazeGameWithoutArtifactsThrows()
    {
        Assert.Throws<ArgumentException>(() => new MazeGame(Guid.NewGuid(), Guid.Empty, GetMockArtifacts(0)));
    }

    // TODO here will be more tests added in further issues
    private IList<MazeArtifact> GetMockArtifacts(int numberOfArtifacts)
    {
        IList<MazeArtifact> artifacts = new List<MazeArtifact>();
        for (int i = 0; i < numberOfArtifacts; i++)
        {
            artifacts.Add(new MazeArtifact(Guid.NewGuid(), $"artifact{i}", $"description{i}"));
        }

        return artifacts;
    }
}
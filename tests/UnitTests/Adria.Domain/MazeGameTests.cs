using System.Collections.Immutable;
using Adria.Domain.games;
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

    // TODO here will be more tests added in further issues
    private static ImmutableHashSet<MazeArtifact> GetMockArtifacts(int numberOfArtifacts)
    {
        HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();
        for (int i = 0; i < numberOfArtifacts; i++)
        {
            artifacts.Add(new MazeArtifact(Guid.NewGuid(), $"artifact{i}", $"description{i}"));
        }

        return artifacts.ToImmutableHashSet();
    }
}
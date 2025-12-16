using Adria.Domain.games;

namespace UnitTests.Mocks;

public static class MockHelpers
{
    public static HashSet<MazeArtifact> GenerateMockArtifacts(int numberOfArtifacts)
    {
        HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();
        for (int i = 0; i < numberOfArtifacts; i++)
        {
            artifacts.Add(new MazeArtifact(Guid.NewGuid(), $"artifact{i}", $"description{i}"));
        }

        return artifacts;
    }
}
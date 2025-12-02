using System.Collections.ObjectModel;
using Adria.Application.Contracts;
using Adria.Domain.games;

namespace UnitTests.Mocks;

public class MockArtifactsQuery : IArtifactsQuery
{
    public Guid GameWithoutArtifactsId { get;  } = Guid.NewGuid();
    public Task<ReadOnlyCollection<MazeArtifact>> Fetch(Guid id)
    {
        IList<MazeArtifact> artifacts = new List<MazeArtifact>();

        if (id != GameWithoutArtifactsId)
        {
            for (int i = 0; i < 15; i++)
            {
                artifacts.Add(new MazeArtifact(Guid.NewGuid(), $"artifact{i}", $"description{i}"));
            }
        }
        
        return Task.FromResult(artifacts.AsReadOnly());
    }
}
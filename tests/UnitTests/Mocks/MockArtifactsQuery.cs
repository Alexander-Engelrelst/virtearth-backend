using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Adria.Application.Contracts;
using Adria.Domain.games;

namespace UnitTests.Mocks;

public class MockArtifactsQuery : IArtifactsQuery
{
    public Guid GameWithoutArtifactsId { get;  } = Guid.NewGuid();
    public Task<IReadOnlySet<MazeArtifact>> Fetch(Guid id)
    {
        HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();

        if (id != GameWithoutArtifactsId)
        { 
            artifacts.UnionWith(MockHelpers.GenerateMockArtifacts(15));
        }
        
        return Task.FromResult<IReadOnlySet<MazeArtifact>>(artifacts.ToImmutableHashSet());
    }
}
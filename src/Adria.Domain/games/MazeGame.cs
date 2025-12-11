using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;

namespace Adria.Domain.games;

public class MazeGame : Game
{   
    /* normally this would be more intricate since each different maze game would have a different layout
     * and a different amount of artifacts, for simplicity I will not be implementing this immediately
     * if this wasn't fixed it means that there was insufficient time to add this properly */
    
    public IMazeElement?[,] Maze { get; }
    private readonly ISet<MazeArtifact> _foundArtifacts = new HashSet<MazeArtifact>();

    public IReadOnlySet<MazeArtifact> FoundArtifacts => _foundArtifacts.ToImmutableHashSet();
    public IReadOnlySet<MazeArtifact> Artifacts { get; }
    public (int xCord, int yCord)? ExitCoordinates = null;
    public MazeGame(Guid gameId, User user, IReadOnlySet<MazeArtifact> artifacts) : base(gameId, user)
    {
        if (artifacts.Count == 0) throw new ArgumentOutOfRangeException(nameof(artifacts), "Must have at least 1 artifact");
        
        Maze = MazeGenerator.GenerateMaze(artifacts);
        Artifacts = artifacts;
    }

    public MazeGame? UpdateUserFoundArtifacts(Guid inputArtifactId, float xCord, float yCord, float angle)
    {
        if (_foundArtifacts.Select(artifact => artifact.Id).Contains(inputArtifactId))
        {
            throw new ArtifactAlreadyFoundException(User.Id, inputArtifactId);
        }
        
        MazeArtifact artifact = Artifacts.FirstOrDefault(artifact => artifact.Id == inputArtifactId)
            ?? throw new ArtifactNotFoundException(User.Id, inputArtifactId, GameId); 
        
        _foundArtifacts.Add(artifact);

        if (_foundArtifacts.Count != Artifacts.Count) return null;

        ExitCoordinates = MazeGenerator.GenerateMazeExit(Maze, xCord, yCord, angle);
        return this;
    }
}
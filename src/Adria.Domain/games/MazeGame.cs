using System.Text.Json.Serialization;

namespace Adria.Domain.games;

public class MazeGame : Game
{   
    /* normally this would be more intricate since each different maze game would have a different layout
     * and a different amount of artifacts, for simplicity I will not be implementing this immediately
     * if this wasn't fixed it means that there was insufficient time to add this properly */
    
    public MazeElement?[,] Maze { get; }
    private ISet<MazeArtifact> FoundArtifacts { get; } = new HashSet<MazeArtifact>();
    private ISet<MazeArtifact> Artifacts { get; } = new HashSet<MazeArtifact>();
    public MazeGame(Guid gameId, Guid userId, IList<MazeArtifact> artifacts) : base(gameId, userId)
    {
        if (artifacts.Count < 1) throw new ArgumentOutOfRangeException(nameof(artifacts), "Must have at least 1 artifact");
        
        Maze = MazeGenerator.GenerateMaze(artifacts);
        foreach (MazeArtifact artifact in artifacts)
        {
            Artifacts.Add(artifact);
        }
    }

}
namespace Adria.Domain.games;

public record MazeGameData(Guid Id, string Name);

public class MazeGame : Game

{   private const int MinimumCellsBetweenArtifacts = 5;
    private const double MazeSizeSafetyFactor = 2;
    
    /* normally this would be more intricate since each different maze game would have a different layout
     * and a different amount of artifacts, for simplicity I will not be implementing this immediately
     * if this wasn't fixed it means that there was insufficient time to add this properly */
    public MazeElement?[,] Maze { get; }
    private ISet<MazeArtifact> FoundArtifacts { get; } = new HashSet<MazeArtifact>();
    private ISet<MazeArtifact> Artifacts { get; } = new HashSet<MazeArtifact>();
    public MazeGame(Guid gameId, Guid userId, IList<MazeArtifact> artifacts) : base(gameId, userId)
    {
        Maze = MazeGenerator.GenerateMaze(artifacts, MinimumCellsBetweenArtifacts);
        foreach (MazeArtifact artifact in artifacts)
        {
            Artifacts.Add(artifact);
        }
    }
    
    public string MazeToString()
    {
        int width = Maze.GetLength(0);
        int height = Maze.GetLength(1);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Maze[x, y] == null)
                {
                    sb.Append(" "); // empty / path
                }
                else if (Maze[x, y] is MazeWall)
                {
                    sb.Append("W"); // wall
                }
                else if (Maze[x, y] is MazeArtifact)
                {
                    sb.Append("A"); // artifact
                }
                else
                {
                    sb.Append("?"); // unknown element
                }
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

}
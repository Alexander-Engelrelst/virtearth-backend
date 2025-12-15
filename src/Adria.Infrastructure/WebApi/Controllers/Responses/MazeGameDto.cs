using Adria.Domain.games;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed class MazeGameDto
{
    private const int MAZE_WALKABLE_PATH_NUMBER = 0;
    private const int MAZE_WALL_NUMBER = 1;
    private const int MAZE_EXIT_NUMBER = 99;
    public Guid GameId { get; set; }
    // for some reason serialization doesn't allow int[,]
    public int[][] Maze { get; set; }
    public IList<MazeArtifactDto> Artifacts { get; } = new List<MazeArtifactDto>();

    public MazeGameDto(MazeGame game)
    {
        GameId = game.GameId;
        
        
        IMazeElement?[,] maze = game.Maze;
        
        Maze = new int[maze.GetLength(0)][];
        
        for (int i = 0; i < game.Maze.GetLength(0); i++)
        {
            Maze[i] = new int[maze.GetLength(1)];

            for (int j = 0; j < game.Maze.GetLength(1); j++)
            {
                switch(maze[i, j])
                {
                    case null:
                        Maze[i][j] = MAZE_WALKABLE_PATH_NUMBER;
                        break;
                    case MazeWall:
                        Maze[i][j] = MAZE_WALL_NUMBER;
                        break;
                    case MazeArtifact:
                        Maze[i][j] = MAZE_WALKABLE_PATH_NUMBER;
                        Artifacts.Add(new MazeArtifactDto((MazeArtifact) maze[i, j]!, i , j));
                        break;
                    case MazeGameExit:
                        Maze[i][j] = MAZE_EXIT_NUMBER;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(game),
                            maze[i, j],
                            "Unexpected element found in the maze.");
                }
            }
        }
    }
}
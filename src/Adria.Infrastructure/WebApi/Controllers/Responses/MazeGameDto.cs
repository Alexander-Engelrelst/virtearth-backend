using Adria.Domain.games;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed class MazeGameDto
{
    public Guid GameId { get; set; }
    // for some reason serialization doesn't allow int[,]
    public int[][] Maze { get; set; }
    public IList<MazeArtifactDto> Artifacts { get; } = new List<MazeArtifactDto>();
    public CoordinatesDto? Coordinates { get; set; }

    public MazeGameDto(MazeGame game)
    {
        GameId = game.GameId;
        Coordinates = game.ExitCoordinates is null ? null : new CoordinatesDto(game.ExitCoordinates.Value.xCord, game.ExitCoordinates.Value.yCord);
        
        
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
                        Maze[i][j] = 0;
                        break;
                    case MazeWall:
                        Maze[i][j] = 1;
                        break;
                    case MazeArtifact:
                        Maze[i][j] = 0;
                        Artifacts.Add(new MazeArtifactDto((MazeArtifact) maze[i, j]!, i , j));
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
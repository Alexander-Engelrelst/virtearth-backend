using System.Diagnostics;
using Adria.Domain.games;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

// TODO implement this shit
public sealed class MazeGameDto
{
    public Guid GameId { get; set; }
    // for some reason serialization doesn't allow int[,]
    public int[][] Maze { get; set; }
    
    public MazeGameDto(MazeGame game)
    {
        GameId = game.GameId;
        
        MazeElement?[,] maze = game.Maze;
        
        Maze = new int[maze.GetLength(0)][];
        
        for (int i = 0; i < game.Maze.GetLength(0); i++)
        {
            Maze[i] = new int[maze.GetLength(1)];

            for (int j = 0; j < game.Maze.GetLength(1); j++)
            {
                Maze[i][j] = maze[i, j] switch
                {
                    null => 0,
                    MazeArtifact => 0,
                    MazeWall => 1,
                    _ => throw new ArgumentOutOfRangeException(
                        nameof(maze),
                        maze[i, j],
                        "Unexpected element found in the maze.")
                };
            }
        }
    }
}
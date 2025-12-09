using System.Collections.Immutable;
using Adria.Domain.games;
using Adria.Infrastructure.WebApi.Controllers.Responses;

namespace UnitTests.Adria.Infrastructure;

public class GameDtoTests
{
    // this test's sole purpose is to compare a MazeGameDto with the original MazeGame to ensure that everything gets mapped correctly
    [Fact]
    public void MazeGameDtoMazeGetsMappedCorrectly()
    {
        HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();

        for (int i = 0; i < 2; i++)
        {
            artifacts.Add(new MazeArtifact(Guid.NewGuid(), $"artifact{i}", $"description{i}"));
        }
        
        MazeGame game = new(Guid.NewGuid(), new("username"), artifacts.ToImmutableHashSet());
        MazeGameDto dto = new(game);

        IMazeElement?[,] maze = game.Maze;
        int[][] dtoMaze = dto.Maze;

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                switch (maze[i, j])
                {
                    case null:
                        Assert.Equal(0, dtoMaze[i][j]);
                        break;
                    case MazeWall:
                        Assert.Equal(1, dtoMaze[i][j]);
                        break;
                    case MazeArtifact artifact:
                        Assert.Equal(0, dtoMaze[i][j]);
                        Assert.Contains(new MazeArtifactDto(artifact, i, j), dto.Artifacts);
                        break;
                    default:
                        Assert.Fail("Unexpected MazeElement");
                        break;
                }
            } 
        }
    }
}
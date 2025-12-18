using System.Collections.Immutable;
using Adria.Domain.games;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using UnitTests.Mocks;

namespace UnitTests.Adria.Infrastructure;

public class GameDtoTests
{
    // this test's sole purpose is to compare a MazeGameDto with the original MazeGame to ensure that everything gets mapped correctly
    [Fact]
    public void MazeGameDtoMazeGetsMappedCorrectly()
    {
        HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();
    
        Guid artifactId = Guid.NewGuid();
        artifacts.Add(new MazeArtifact(artifactId, "name", "description"));
        
        MazeGame game = new(Guid.NewGuid(), new("username"), artifacts.ToImmutableHashSet());
        game.UpdateUserFoundArtifacts(artifactId);
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
                    case MazeGameSpawn:
                        Assert.Equal(0, dtoMaze[i][j]);
                        Assert.Equal(i + 0.5f, dto.SpawnLocation.X);
                        Assert.Equal(j + 0.5f, dto.SpawnLocation.Y);
                        break;
                    case MazeGameExit:
                        Assert.Equal(2, dtoMaze[i][j]);
                        break;
                    default:
                        Assert.Fail("Unexpected MazeElement");
                        break;
                }
            } 
        }
    }
}
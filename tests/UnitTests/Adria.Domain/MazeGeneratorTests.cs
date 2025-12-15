using System.Collections.Immutable;
using Adria.Domain.games;
using UnitTests.Mocks;

namespace UnitTests.Adria.Domain;

public class MazeGeneratorTests
{ 
    /* yes this test was written by chatgpt,
     * yes there was absolutely no way I wanted to write this by myself and yes I read it to make sure it works
     * this test checks which cells are walkable places, it chooses a starting point and just keeps walking all paths
     * until it has no more places left to get to, if somehow the algorithm has a place to go to
     * that isn't the cell it visited in the iteration before but it was visited already then the maze has a cycle
     * and if it has a cycle it isn't a perfect maze*/
    [Fact]
    public void Maze_ShouldBeAcyclicAndFullyConnected()
    {
        for (int count = 0; count < 10; count++)
        {
            HashSet<MazeArtifact> artifacts = new HashSet<MazeArtifact>();
            HashSet<MazeArtifact> artifactsFoundInMaze = new HashSet<MazeArtifact>();
            
            artifacts.UnionWith(MockHelpers.GenerateMockArtifacts(100));
            
            var maze = MazeGenerator.GenerateMaze(artifacts.ToImmutableHashSet());
            int rows = maze.GetLength(0);
            int cols = maze.GetLength(1);
    
            bool IsWalkable(int r, int c)
                => maze[r, c] is null or MazeArtifact;
    
            var directions = new (int dr, int dc)[]
            {
                (1,0), (-1,0), (0,1), (0,-1)
            };
    
            // Count total walkable cells
            int totalWalkable = 0;
            (int r, int c)? start = null;
    
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (IsWalkable(r, c))
                    {
                        totalWalkable++;
                        if (start == null)
                            start = (r, c);
                    }

                    if (maze[r, c] is MazeArtifact artifact)
                    {
                        artifactsFoundInMaze.Add(artifact);
                    }
                }
            }
    
            Assert.NotNull(start);
            Assert.True(totalWalkable > 0);
    
            var visited = new HashSet<(int r, int c)>();
            bool hasCycle = false;
    
            void Dfs((int r, int c) current, (int r, int c)? parent)
            {
                if (hasCycle) return;
                visited.Add(current);
    
                foreach (var (dr, dc) in directions)
                {
                    var next = (current.r + dr, current.c + dc);
    
                    if (next.Item1 < 0 || next.Item1 >= rows ||
                        next.Item2 < 0 || next.Item2 >= cols)
                        continue;
    
                    if (!IsWalkable(next.Item1, next.Item2))
                        continue;
    
                    if (!visited.Contains(next))
                    {
                        Dfs(next, current);
                    }
                    else if (parent != null && next != parent)
                    {
                        hasCycle = true;
                        return;
                    }
                }
            }
    
            // DFS from the first walkable cell
            Dfs(start.Value, null);
    
            // ❌ FAIL if loops exist
            Assert.False(hasCycle, "Maze contains a cycle (loop) and is not a perfect maze.");
    
            // ❌ FAIL if not all walkable cells were reached
            Assert.Equal(
                totalWalkable,
                visited.Count
            );

            foreach (var artifact in artifacts)
            {
                Assert.Contains(artifact, artifactsFoundInMaze);
            }
        }
    }
    
    private static MazeWall _wall = new MazeWall();
    private static MazeGameExit _exit = new();
    
    [Fact]
    public void GenerateExitTestLookingAtAWall270deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall },
            { _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall },
            { _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 1 , 1, 270);
        Assert.IsType<MazeGameExit>(maze[1 , 0]);
    }
    
    [Fact]
    public void GenerateExitTestLookingAtAWall90deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall },
            { _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall },
            { _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 1 , 1, 90);
        Assert.IsType<MazeGameExit>(maze[1 , 2]);
    }
    
    [Fact]
    public void GenerateExitTestLookingAtAWall45deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall },
            { _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall },
            { _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 1 , 1, 45);
        Assert.True(maze[0, 1] is MazeGameExit ^ maze[1, 2] is MazeGameExit);
    }
    
    [Fact]
    public void GenerateExitTestLookingAtAWall30deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[4, 4]
        {
            { _wall, _wall, _wall, _wall },
            { _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall },
            { _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 1 , 1, 45);
        Assert.IsType<MazeGameExit>(maze[0 , 1]);
    }
    
    [Fact]
    public void GenerateExitTestLookingAtEmptySidesAreWalls()
    {
        IMazeElement?[,] maze = new IMazeElement?[4, 4]
        {
            { _wall, _wall, _wall, _wall },
            { _wall, null, null, _wall },
            { _wall, _wall, _wall, _wall },
            { _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 1 , 1, 90);
        Assert.True(maze[0, 1] is MazeGameExit ^ maze[2, 1] is MazeGameExit);
    }
    
    [Fact]
    public void GenerateExitTestLookingAtEmptySidesAreEmpty()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, _wall, null, null, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 2 , 2, 90);
        Assert.IsType<MazeGameExit>(maze[2, 2]);
    }
    
    [Fact]
    public void GenerateExitAllNonDiagonalEmpty90deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, null, null, null, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 2 , 2, 90);
        Assert.True(maze[2, 3] is MazeGameExit ^ maze[2, 1] is MazeGameExit);
    }
    
    [Fact]
    public void GenerateExitAllNonDiagonalEmpty45deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, null, null, null, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 2 , 2, 45);
        Assert.IsType<MazeGameExit>(maze[1, 3]);
    }
    
    [Fact]
    public void GenerateExitAllNonDiagonalEmpty30deg()
    {
        IMazeElement?[,] maze = new IMazeElement?[,]
        {
            { _wall, _wall, _wall, _wall, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, null, null, null, _wall },
            { _wall, _wall, null, _wall, _wall },
            { _wall, _wall, _wall, _wall, _wall }
        };
        
        MazeGenerator.GenerateMazeExit(maze, 2 , 2, 45);
        Assert.IsType<MazeGameExit>(maze[1, 3]);
    }
    
    /* we don't need more tests for diagonal the maze algorithm ensures that if all horizontal spaces are null,
     * then all diagonals will be a wall */
}
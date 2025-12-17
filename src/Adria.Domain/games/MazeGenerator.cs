using Adria.Domain.Shared;

namespace Adria.Domain.games;
#pragma warning disable S2245
// yes using pseudorandom is safe here sonar
public static class MazeGenerator
{
    private const int MINIMUM_CELLS_BETWEEN_ARTIFACTS = 5;
    private const int MAZE_SIZE_SAFETY_FACTOR = 2;

    /*
     * The algorithm to construct a maze consists out of 2 distinct steps.
     * They are adding the actual paths the user will be able to walk and adding the artifacts in the maze
     * Since the rendering engine expects an array where walkable paths are empty and the walls are represented in an array
     * we will use a mapping between the array in the algorithm and the array that we will store for the user
     * for a walkable cell in the maze we will also keep track of the walls around it
     * this means that cordMaze = 2 * cordAlgorithmMaze + 1
     * The algorithm uses a depth first search and backtracking algorithm
     * we will keep track of all visited nodes
     *
     * we start by determining the size of the maze. We do this based on the amount of artifacts to be able to ensure
     * distance between different artifacts in the end result
     *
     * for the maze generation we start with a maze that is completely filled with walls
     * we will pick a random starting point and add it to a stack and set it to visited
     * then we will get all neighbours (not diagonally) (which for the first step is obviously all of them)
     * then we will pick a random neighbour, remove the wall between the cell and the neighbour
     * we will add the neighbour to the stack and set it to visited
     * in the next iteration we will peek from the stack, this will return the neighbour from previous iteration
     * we will repeat the step of getting a neighbour and removing the walls
     * eventually we will get stuck in a situation where there are no unvisited neighbours available anymore
     * in this scenario we will pop from the stack and do a next iteration (this means we will go back to the previous cell)
     * this we can do until the stack is completely empty. In this case every single cell in the maze has been visited
     * this generates a perfect mathematically perfect maze.
     *
     * For the artifacts we will in a loop add each artifact into the maze
     * while doing so we will keep track of all cells that are far enough away of an already added artifact.
     * This is not the most optimized solution but hey bite me
     * We will attempt to add artifacts in a loop
     * It is highly unlikely and maybe even impossible to come into a situation where adding artifacts fail
     * this is because the maze is purposefully made large, but I'd rather be safe than sorry if someone decided to change anything
     */
    public static IMazeElement?[,] GenerateMaze(IReadOnlySet<MazeArtifact> artifacts)
    {
        int size = (int) Math.Sqrt(artifacts.Count * Math.Pow(MINIMUM_CELLS_BETWEEN_ARTIFACTS, 2) * MAZE_SIZE_SAFETY_FACTOR);
        IMazeElement?[,] maze = new IMazeElement?[2 * size + 1, 2 * size + 1];

        FillMazeWithWalls(maze, new MazeWall());
        GenerateWalkablePaths(maze, size);
        bool artifactsAdded;

        int attempts = 0;
        do
        {
            if (attempts > 10)
            {
                throw new MazeGenerationException("Maximum number of attempts to place artifacts exceeded");
            }
            
            artifactsAdded = AddArtifacts(maze, artifacts, MINIMUM_CELLS_BETWEEN_ARTIFACTS);
            attempts++;
        } while (!artifactsAdded);
        
        return maze;
    }
    
    private static void FillMazeWithWalls(IMazeElement?[,] maze, MazeWall wall)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                maze[i, j] = wall;
            }
        }
    }
    private static void GenerateWalkablePaths(IMazeElement?[,] maze, int size)
    {
        bool[,] visitedNodes = new bool[size, size];
        
        Random random = new Random();
        int startingX = random.Next(0, size);
        int startingY = random.Next(0, size);
        
        Stack<(int x, int y)> stack = new Stack<(int x, int y)>();
        
        stack.Push((startingX, startingY));
        visitedNodes[startingX, startingY] = true;
        
        maze[
            2 * startingX + 1,
            2 * startingY + 1
        ] = new MazeGameSpawn();
        while (stack.Count > 0)
        {
            (int x, int y) currentCell = stack.Peek();
            
            List<(int xCord, int yCord)> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell.x, currentCell.y, visitedNodes, size);

            if (unvisitedNeighbours.Count > 0)
            { 
                (int x, int y) neighbour = unvisitedNeighbours[random.Next(unvisitedNeighbours.Count)];
                stack.Push((neighbour.x, neighbour.y));
                visitedNodes[neighbour.x, neighbour.y] = true;

                maze[
                    ((2 * currentCell.x + 1) + (2 * neighbour.x + 1)) / 2,
                    ((2 * currentCell.y + 1) + (2 * neighbour.y + 1)) / 2
                ] = null;

                maze[
                    2 * neighbour.x + 1,
                    2 * neighbour.y + 1
                ] = null;
            }
            else
            {
                stack.Pop();
            }
        }

    }

    private static List<(int xCord, int yCord)> GetUnvisitedNeighbours(int x, int y, bool[,] visitedNodes, int size)
    {
        List<(int xCord, int yCord)> unVisitedNeighbours = [];
        
        if (y > 0 && !visitedNodes[x, y - 1]) unVisitedNeighbours.Add((x, y - 1));
        if (y < size - 1 && !visitedNodes[x, y + 1]) unVisitedNeighbours.Add((x, y + 1));
        if (x > 0 && !visitedNodes[x - 1, y]) unVisitedNeighbours.Add((x - 1, y));
        if (x < size - 1 && !visitedNodes[x + 1, y]) unVisitedNeighbours.Add((x + 1, y));
        
        return unVisitedNeighbours;
    }

    private static bool AddArtifacts(IMazeElement?[,] maze, IReadOnlySet<MazeArtifact> artifacts, int minimumCellsBetweenArtifacts)
    {
        bool[,] availableSpaces = GetBaseAvailableSpaces(maze);
        Random random = new Random();
        int size = availableSpaces.GetLength(0);
        IDictionary<MazeArtifact, (int xCord, int yCord)> artifactsCoordinates 
            = new Dictionary<MazeArtifact, (int xCord, int yCord)>();
        
        foreach (MazeArtifact artifact in  artifacts)
        {
            if (NoMoreAvailableSpots(availableSpaces))
            {
                return false;
            }
            
            (int x, int y) coordinates;

            do
            {
                coordinates = (random.Next(1, size - 1), random.Next(1, size - 1));
            } while (!availableSpaces[coordinates.x , coordinates.y]);
            
            artifactsCoordinates[artifact] = coordinates;
            MakeSpotsUnavailable(availableSpaces, coordinates, minimumCellsBetweenArtifacts);
        }

        foreach (KeyValuePair<MazeArtifact, (int xCord, int yCord)> kvp in artifactsCoordinates)
        {
            maze[kvp.Value.xCord, kvp.Value.yCord] = kvp.Key;
        }
        return true;
    }

    private static bool NoMoreAvailableSpots(bool[,] availableSpaces)
    {
        for (int i = 0; i < availableSpaces.GetLength(0); i++)
        {
            for (int j = 0; j < availableSpaces.GetLength(1); j++)
            {
                if (availableSpaces[i, j]) return false;
            }
        }

        return true;
    }

    private static void MakeSpotsUnavailable(bool[,] availableSpaces, (int x, int y) coordinates, int stepsRemaining)
    {
        if (stepsRemaining == 0) return;
        int size = availableSpaces.GetLength(0);
        
        List<(int x, int y)> neighbours = [];
        if (coordinates.x > 0) neighbours.Add((coordinates.x - 1, coordinates.y));
        if (coordinates.x < size - 1) neighbours.Add((coordinates.x + 1, coordinates.y));
        if (coordinates.y > 0) neighbours.Add((coordinates.x, coordinates.y - 1));
        if (coordinates.y < size - 1) neighbours.Add((coordinates.x, coordinates.y + 1));
        
        // I am aware that this is far from the most ideal way to do it but hey bite me
        foreach ((int xCord, int yCord) neighbour in neighbours)
        {
            availableSpaces[neighbour.xCord, neighbour.yCord] = false;
            MakeSpotsUnavailable(availableSpaces, neighbour, stepsRemaining - 1);
        }
    }

    private static bool[,] GetBaseAvailableSpaces(IMazeElement?[,] maze)
    {
        bool[,] spaces = new bool[maze.GetLength(0), maze.GetLength(1)];
        
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] is null)
                {
                    spaces[i, j] = true;
                }
            }
        }
        
        return spaces;
    }

    #pragma warning disable S2368
    // (ง ͠° ͟ل͜ ͡°)ง  I will use a multidimensional array wherever I want >:(
    public static void GenerateMazeExit(IMazeElement?[,] maze, float xCord, float yCord, float angleDeg)
    {
        angleDeg = (angleDeg % 360 + 360) % 360;
        int roundedXCord = (int)Math.Floor(xCord);
        int roundedYCord = (int)Math.Floor(yCord);
        
        if (maze[roundedXCord, roundedYCord] is MazeWall)
        {
            throw new ArgumentException("at least one of the coordinates is invalid");
        }
        
        var directions = new (int dx, int dy, float angle)[]
        {
            (-1, 0, 0f),
            (0, 1, 90f),
            (1, 0, 180f),
            (0, -1, 270f)
        };
        
        var orderedValidDirections = directions
            .Where(d => maze[roundedXCord + d.dx, roundedYCord + d.dy] is MazeWall)
            .OrderBy(d => AngleDifference(angleDeg, d.angle))
            .ToArray();

        if (orderedValidDirections.Length > 0)
        {
            var dir = orderedValidDirections[0];
            maze[roundedXCord + dir.dx, roundedYCord + dir.dy] = new MazeGameExit();
            return;
        }
        
        var diagonalDirections = new (int dx, int dy, float angle)[]
        {
            (-1, 1, 45f),
            (1, 1, 135f),
            (1, -1, 225f),
            (-1, -1, 315f),
        };

        var chosenDir = diagonalDirections
            .OrderBy(d => AngleDifference(angleDeg, d.angle))
            .First();
        
        maze[roundedXCord + chosenDir.dx, roundedYCord + chosenDir.dy] = new MazeGameExit();
    }
    
    #pragma warning restore S2368

    private static float AngleDifference(float angle1, float angle2)
    {
        var diff = Math.Abs(angle1 - angle2) % 360;
        return diff > 180 ? 360 - diff : diff;
    }
}
#pragma warning restore S2245


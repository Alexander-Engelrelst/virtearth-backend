namespace Adria.Domain.games;

public class MazeGame : Game
{
    private const int MinimumCellsBetweenArtifacts = 5;
    private const double MazeSizeSafetyFactor = 2;
    
    /* normally this would be more intricate since each different maze game would have a different layout
     * and a different amount of artifacts, for simplicity I will not be implementing this immediately
     * if this wasn't fixed it means that there was insufficient time to add this properly */
    public MazeElement?[,] Maze { get; }
    private ISet<MazeArtifact> FoundArtifacts { get; } = new HashSet<MazeArtifact>();
    private ISet<MazeArtifact> Artifacts { get; } = new HashSet<MazeArtifact>();
    public MazeGame(Guid gameId, Guid userId, IList<MazeArtifact> artifacts) : base(gameId, userId)
    {
        // we need the maze to be large enough to ensure we can place artifacts far enough from each other
        int size = (int) Math.Sqrt(artifacts.Count * Math.Pow(MinimumCellsBetweenArtifacts, 2) * MazeSizeSafetyFactor);
        
        Maze = GenerateMaze(size, artifacts);
    }

    private static MazeElement?[,] GenerateMaze(int size, IList<MazeArtifact> artifacts)
    {
        /* since our rendering engine expects an array where walls must be represented we will conform to this
         * we will be using a DFS with recursive backtracking algorithm to generate our mazes where only uneven cells
         * are treated as actual cells (uneven x and y coordinate) this leaves space in between to add the wall needed
         * by our rendering engine */
        
        MazeElement?[,] maze = new MazeElement?[2 * size + 1, 2 * size + 1];

        FillMazeWithWalls(maze, new MazeWall());
        GenerateWalkablePaths(maze, size);
        bool artifactsAdded;


        do
        {
            artifactsAdded = AddArtifacts(maze, artifacts);
        } while (!artifactsAdded);
        
        return maze;
    }

    /* we use the same wall over and over again to save memory, a wall is immutable anyway
     * this is especially useful for larger mazes */
    private static void FillMazeWithWalls(MazeElement?[,] maze, MazeWall wall)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                maze[i, j] = wall;
            }
        }
    }
    /* for easier use we will use visitednodes as the regular size
     * because this is the main array that will be used to know where we have already gone in our algorithm
     * we can easily map these two arrays using mazeindex = 2 * visitednodeIndex + 1*/
    private static void GenerateWalkablePaths(MazeElement?[,] maze, int size)
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
        ] = null;
        while (stack.Count > 0)
        {
            (int x, int y) currentCell = stack.Peek();
            
            IList<(int xCord, int yCord)> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell.x, currentCell.y, visitedNodes, size);

            if (unvisitedNeighbours.Count > 0)
            { 
                (int x, int y) neighbour = unvisitedNeighbours[random.Next(unvisitedNeighbours.Count)];
                stack.Push((neighbour.x, neighbour.y));
                visitedNodes[neighbour.x, neighbour.y] = true;
                Console.WriteLine(currentCell.x + " " + currentCell.y + " " + neighbour.x + " " + neighbour.y);
                // we map our visited nodes to our maze nodes to remove the wall
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
                // if there are no neighbours left we will backtrack to a previous cell with neighbours to visit
                stack.Pop();
            }
        }

    }

    private static IList<(int xCord, int yCord)> GetUnvisitedNeighbours(int x, int y, bool[,] visitedNodes, int size)
    {
        IList<(int xCord, int yCord)> unVisitedNeighbours = new List<(int xCord, int yCord)>();
        
        if (y > 0 && !visitedNodes[x, y - 1]) unVisitedNeighbours.Add((x, y - 1));
        if (y < size - 1 && !visitedNodes[x, y + 1]) unVisitedNeighbours.Add((x, y + 1));
        if (x > 0 && !visitedNodes[x - 1, y]) unVisitedNeighbours.Add((x - 1, y));
        if (x < size - 1 && !visitedNodes[x + 1, y]) unVisitedNeighbours.Add((x + 1, y));
        
        return unVisitedNeighbours;
    }

    private static bool AddArtifacts(MazeElement?[,] maze, IList<MazeArtifact> artifacts)
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
            MakeSpotsUnavailable(availableSpaces, coordinates, MinimumCellsBetweenArtifacts);
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
        
        IList<(int x, int y)> neighbours = new List<(int x, int y)>();
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

    private static bool[,] GetBaseAvailableSpaces(MazeElement?[,] maze)
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
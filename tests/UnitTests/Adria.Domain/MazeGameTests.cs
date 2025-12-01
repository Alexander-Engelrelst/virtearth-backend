using Adria.Domain.games;
using Xunit.Abstractions;

namespace UnitTests.Adria.Domain;

public class MazeGameTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MazeGameTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    // TODO write actual tests lol (maybe a flood algorithm to check if every artifacts is reachable from any starting point in the maze
    // replacing visited artifacts with a visitied (dummy data type) check if all artifacts are gone in the end and all 
    // visitable nodes have been visited
}
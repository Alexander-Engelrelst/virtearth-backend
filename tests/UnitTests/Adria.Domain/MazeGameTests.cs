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
    
    // TODO write actuall tests lol
    [Fact]
    public void thisIsATest()
    {
        IList<MazeArtifact> artifacts = new List<MazeArtifact>();
        for (int i = 0; i < 100; i++)
        {
            artifacts.Add(new MazeArtifact());
        }
        MazeGame game = new MazeGame(Guid.NewGuid(), Guid.NewGuid(), artifacts);
        _testOutputHelper.WriteLine(game.MazeToString());
        Assert.True(true);
    }
}
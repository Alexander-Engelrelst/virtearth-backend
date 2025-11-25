using Adria.Domain.games;

namespace UnitTests.Adria.Domain;

public class GameLocationTests
{
    [Fact]
    public void InvalidLatituteTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameLocation(Guid.NewGuid(), "DitIsEenPrachtigeNaam", 91.00001, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameLocation(Guid.NewGuid(), "DitIsEenPrachtigeNaam", -91.00001, 0));
    }
    
    [Fact]
    public void InvalidLongituteTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameLocation(Guid.NewGuid(), "DitIsEenPrachtigeNaam", 0, 180.00001));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GameLocation(Guid.NewGuid(), "DitIsEenPrachtigeNaam", 0, -180.00001));
    }

    [Fact]
    public void EmptyGuidTest()
    {
        Assert.Throws<ArgumentNullException>(() => new GameLocation(Guid.Empty, "DitIsEenPrachtigeNaam", 0, 0));
    }

    [Fact]
    public void InvalidNameTest()
    {
        Assert.Throws<ArgumentException>(() => new GameLocation(Guid.NewGuid(), "", 0, 0));
        Assert.Throws<ArgumentException>(() => new GameLocation(Guid.NewGuid(), "   ", 0, 0));
    }
}
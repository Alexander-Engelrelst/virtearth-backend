using Adria.Domain.games;

namespace UnitTests.Adria.Domain;

public class GameLocationTests
{
    [Fact]
    public void EmptyGuidThrows()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new GameLocation(Guid.Empty, "name", 0, 0, Continent.Africa, 0)
        );
        Assert.Equal("gameId", exception.ParamName);
    }

    [Fact]
    public void EmptyNameThrows()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new GameLocation(Guid.NewGuid(), string.Empty, 0, 0, Continent.Africa, 0)
        );
        Assert.Equal("gameName", exception.ParamName);
    }

    [Fact]
    public void WhitespaceNameThrows()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new GameLocation(Guid.NewGuid(), " ", 0, 0, Continent.Africa, 0)
        );
        Assert.Equal("gameName", exception.ParamName);
    }

    [Fact]
    public void TooLowLatituteThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameLocation(Guid.NewGuid(), "name", -90.01, 0, Continent.Africa, 0)
        );
        
        Assert.Equal("latitude", exception.ParamName);
    }

    [Fact]
    public void TooHighLatituteThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameLocation(Guid.NewGuid(), "name", 90.01, 0, Continent.Africa, 0)
        );
        Assert.Equal("latitude", exception.ParamName);
    }

    [Fact]
    public void TooLowLongitudeThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameLocation(Guid.NewGuid(), "name", 0, -180.01, Continent.Africa, 0)
        );
        
        Assert.Equal("longitude", exception.ParamName);
    }

    [Fact]
    public void TooHighLongitudeThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new GameLocation(Guid.NewGuid(), "name", 0, 180.01, Continent.Africa, 0)
        );
        
        Assert.Equal("longitude", exception.ParamName);
    }
}
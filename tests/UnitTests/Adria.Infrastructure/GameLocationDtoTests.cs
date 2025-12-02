using Adria.Domain.games;
using Adria.Infrastructure.WebApi.Controllers.Responses;

namespace UnitTests.Adria.Infrastructure;

public class GameLocationDtoTests
{
    [Fact]
    public void OneWordContinentStaysCorrect()
    {
        GameLocation location = new(Guid.NewGuid(), "gameName", 0,0, Continent.Europe, 0);
        GameLocationDto dto =  new(location);
        Assert.Equal("Europe", dto.Continent);
    }
    
    [Fact]
    public void MultiWordContinentGetsSplit()
    {
        GameLocation location = new(Guid.NewGuid(), "gameName", 0,0, Continent.NorthAmerica, 0);
        GameLocationDto dto =  new(location);
        Assert.Equal("North America", dto.Continent);
    }
}
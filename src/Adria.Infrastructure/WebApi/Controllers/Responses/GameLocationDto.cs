using System.Text.RegularExpressions;
using Adria.Domain.games;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed partial class GameLocationDto
{
    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex ContinentFormattingRegex(); 
    public Guid GameId { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    
    public string Continent { get; }
    public int Year { get; }
    public string GameName { get; }
    
    public bool Completed { get; }
    
    public string Description { get; }

    public GameLocationDto(GameLocation gameLocation)
    {
        GameId = gameLocation.GameId;
        Latitude = gameLocation.Latitude;
        Longitude = gameLocation.Longitude;
        Continent = ContinentFormattingRegex().Replace(gameLocation.Continent.ToString(), " $1");
        Year = gameLocation.Year;
        GameName = gameLocation.GameName;
        Completed = gameLocation.Completed;
        Description = gameLocation.Description;
    }
}
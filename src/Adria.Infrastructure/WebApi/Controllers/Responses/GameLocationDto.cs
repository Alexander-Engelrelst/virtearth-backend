using System.Text.RegularExpressions;
using Adria.Domain.games;
using Microsoft.OpenApi.Extensions;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public class GameLocationDto
{
    public Guid GameId { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    
    public string Continent { get; }
    public int Year { get; }
    public string GameName { get; }

    public GameLocationDto(GameLocation gameLocation)
    {
        GameId = gameLocation.GameId;
        Latitude = gameLocation.Latitude;
        Longitude = gameLocation.Longitude;
        Continent = Regex.Replace(gameLocation.Continent.ToString(), "(?<!^)([A-Z])", " $1");
        Year = gameLocation.Year;
        GameName = gameLocation.GameName;
    }
}
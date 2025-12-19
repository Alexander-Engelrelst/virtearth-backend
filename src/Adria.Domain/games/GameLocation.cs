namespace Adria.Domain.games;
// I will use as many constructor parameters as I want, and this was added last minute so I did not have time to fix this
#pragma warning disable S107
public sealed class GameLocation
{
    public Guid GameId { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    
    public Continent Continent { get; }
    public int Year { get; }
    public string GameName { get; }
    
    public bool Completed { get; }
    
    public string Description { get; }
    
    public GameLocation(
        Guid gameId,
        string gameName ,
        double latitude,
        double longitude,
        Continent continent,
        int year,
        string description = "",
        bool completed = false)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentException("gameId cannot be empty.", nameof(gameId));
        }

        if (string.IsNullOrWhiteSpace(gameName))
        {
            throw new ArgumentException("GameName cannot be empty.", nameof(gameName));
        }

        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");
        }
        
        
        GameId = gameId;
        Latitude = latitude;
        Longitude = longitude;
        GameName = gameName;
        Continent = continent;
        Year = year;
        Completed = completed;
        Description = description;
    }
}
#pragma warning restore S2245
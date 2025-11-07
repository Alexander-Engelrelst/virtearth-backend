namespace Adria.Domain.games;

public sealed class GameLocation
{
    public Guid GameId { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    
    public GameLocation(Guid gameId, double latitude, double longitude)
    {
        if (gameId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(gameId), "Game ID cannot be empty.");
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
    }
}
namespace Adria.Domain.Shared;

public class PlayerOutOfBoundsException(Guid gameId, Guid userId, float xCord, float yCord, Exception? innerException = null)
    :Exception($"User {userId} is out of bounds for game {gameId} with coordinates ({xCord}, {yCord}))", innerException)
{
    
}
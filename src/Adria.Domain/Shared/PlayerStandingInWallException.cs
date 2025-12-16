namespace Adria.Domain.Shared;

public class PlayerStandingInWallException(Guid gameId, Guid userId, float x, float y, Exception? innerException = null)
    :Exception($"User {userId} is claiming to be standing on a wall in game {gameId} at coordinates ({x}{y})", innerException)
{
    
}
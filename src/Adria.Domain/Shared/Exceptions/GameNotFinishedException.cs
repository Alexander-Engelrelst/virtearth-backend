namespace Adria.Domain.Shared.Exceptions;

public class GameNotFinishedException(Guid gameId, Guid userId, Exception? innerException = null) 
    : Exception($"Game {gameId} for user {userId} is not finished yet", innerException)
{
    
}
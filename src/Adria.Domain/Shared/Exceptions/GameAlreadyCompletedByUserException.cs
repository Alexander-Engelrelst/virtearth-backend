namespace Adria.Domain.Shared.Exceptions;

public class GameAlreadyCompletedByUserException(Guid gameId, Guid userId, Exception? innerException = null) 
    : Exception($"User {userId} already completed game {gameId}", innerException)
{
}
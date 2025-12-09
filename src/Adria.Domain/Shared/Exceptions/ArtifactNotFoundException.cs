namespace Adria.Domain.Shared.Exceptions;

public class ArtifactNotFoundException(Guid userId, Guid artifactId, Guid gameId, Exception? innerException = null) 
    : ElementNotFoundException($"User {userId} is trying to find a non-existing artifact {artifactId} in {gameId}",innerException)
{
    
}
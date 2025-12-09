namespace Adria.Domain.Shared.Exceptions;

public class ArtifactAlreadyFoundException(Guid userId, Guid artifactId, Exception? innerException = null)
    : Exception($"User {userId} already found artifact {artifactId}", innerException);
namespace Adria.Domain.Shared.Exceptions;

public sealed class MazeGenerationException(string message, Exception? innerException = null)
    : Exception(message, innerException);
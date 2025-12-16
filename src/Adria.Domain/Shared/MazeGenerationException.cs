namespace Adria.Domain.Shared;

public sealed class MazeGenerationException(string message, Exception? innerException = null)
    : Exception(message, innerException);
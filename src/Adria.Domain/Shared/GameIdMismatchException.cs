namespace Adria.Domain.Shared;

public class GameIdMismatchException(Guid userId, Exception? innerException = null) : Exception(
    $"User {userId} is trying to access a game different to the one he's currently playing", innerException);
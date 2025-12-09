namespace Adria.Domain.Shared.Exceptions;

public sealed class ActiveGameNotFoundException(Guid userId, Exception? innerException = null)
    : ElementNotFoundException($"User {userId} is currently not playing a game", innerException)
{
}
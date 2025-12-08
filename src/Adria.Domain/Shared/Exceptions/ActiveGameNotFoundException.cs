namespace Adria.Domain.Shared.Exceptions;

public sealed class ActiveGameNotFoundException(Guid id, Exception? innerException = null)
    : ElementNotFoundException($"User {id} is currently not playing a game", innerException)
{
}
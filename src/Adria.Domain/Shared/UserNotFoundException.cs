namespace Adria.Domain.Shared;

public sealed class UserNotFoundException(Guid id, Exception? innerException = null)
    : ElementNotFoundException($"User with id {id} not found", innerException)
{
    
}
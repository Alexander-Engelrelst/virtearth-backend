namespace Adria.Domain.Shared.Exceptions;

public sealed class UsernameAlreadyExistsException(string userName, Exception? innerException = null) : Exception(
    $"username {userName} already in use."
    , innerException)
{
}
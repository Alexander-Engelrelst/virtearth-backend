namespace Adria.Domain.Shared.Exceptions;

public class UsernameAlreadyExistsException(string userName, Exception? innerException = null) : Exception(
    $"username {userName} already in use."
    , innerException)
{
}
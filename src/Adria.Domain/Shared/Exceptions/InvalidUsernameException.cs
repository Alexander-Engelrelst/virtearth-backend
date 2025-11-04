namespace Adria.Domain.Shared.Exceptions;

public class InvalidUsernameException(string userName, Exception? innerException = null) : Exception(
    $"Invalid username: {userName}: must be between 3 and 40 characters long and may only contain a-zA-Z0-9 and -_. character."
    , innerException)
{
}
namespace Adria.Domain.Shared;

public class NoUserIdInTokenException(Exception? innerException)
    : Exception("The is no user id in the JWT token", innerException)
{
    public NoUserIdInTokenException() : this(null){}
}
namespace Adria.Infrastructure.Persistence.Shared;

public class DuplicatePrimaryKeyException(string message, Exception? innerException = null) 
    : Exception(message, innerException)
{
}
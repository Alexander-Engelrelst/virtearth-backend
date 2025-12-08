namespace Adria.Domain.Shared.Exceptions;
public abstract class ElementNotFoundException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
}
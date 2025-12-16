namespace Adria.Domain.Shared;
public abstract class ElementNotFoundException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
}
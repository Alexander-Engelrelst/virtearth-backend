namespace Adria.Infrastructure.Persistence.Shared;

public sealed class VirtEarthDatabaseException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public VirtEarthDatabaseException()
        : this("An error occurred while accessing the VirtEarth database.")
    {
    }

    public VirtEarthDatabaseException(Exception innerException)
        : this("An error occurred while accessing the VirtEarth database.", innerException)
    {
    }
}
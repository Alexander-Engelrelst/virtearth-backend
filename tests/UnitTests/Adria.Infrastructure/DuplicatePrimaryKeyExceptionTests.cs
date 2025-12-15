using Adria.Infrastructure.Persistence.Shared;

namespace UnitTests.Adria.Infrastructure;

public class DuplicatePrimaryKeyExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var message = "this is a very useful message";
        
        // Act
        var exception = new DuplicatePrimaryKeyException(message);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var message = "this is a very useful message";
        var innerException = new Exception("Inner exception");
        
        // Act
        var exception = new DuplicatePrimaryKeyException(message, innerException);
        
        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
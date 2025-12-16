using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class NoUserIdInTokenExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Act
        var exception = new NoUserIdInTokenException();

        // Assert
        Assert.Equal(
            "The is no user id in the JWT token", exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        
        // Act
        var exception = new NoUserIdInTokenException(innerException);

        // Assert
        Assert.Equal(
            "The is no user id in the JWT token", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}
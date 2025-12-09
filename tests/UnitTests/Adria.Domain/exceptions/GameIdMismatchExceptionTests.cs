using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Adria.Domain.exceptions;

public class GameIdMismatchExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act
        var exception = new GameIdMismatchException(userId);

        // Assert
        Assert.Equal(
            $"User {userId} is trying to access a game different to the one he's currently playing", exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var userId = Guid.NewGuid();
        
        // Act
        var exception = new GameIdMismatchException(userId, innerException);

        // Assert
        Assert.Equal(
            $"User {userId} is trying to access a game different to the one he's currently playing", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}
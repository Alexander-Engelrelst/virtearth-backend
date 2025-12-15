using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Adria.Domain.exceptions;

public class GameAlreadyCompletedExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        
        // Act
        var exception = new GameAlreadyCompletedByUserException(gameId, userId);

        // Assert
        Assert.Equal(
            $"User {userId} already completed game {gameId}", exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        
        // Act
        var exception = new GameAlreadyCompletedByUserException(gameId, userId, innerException);

        // Assert
        Assert.Equal(
            $"User {userId} already completed game {gameId}", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}
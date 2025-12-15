using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Adria.Domain.exceptions;

public class GameNotFinishedExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gameId =  Guid.NewGuid();
        // Act
        var exception = new GameNotFinishedException(gameId, userId);

        // Assert
        Assert.Equal(
            $"Game {gameId} for user {userId} is not finished yet", exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");
        var userId = Guid.NewGuid();
        var gameId =  Guid.NewGuid();
        // Act
        var exception = new GameNotFinishedException(gameId, userId, innerException);

        // Assert
        Assert.Equal(
            $"Game {gameId} for user {userId} is not finished yet", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}

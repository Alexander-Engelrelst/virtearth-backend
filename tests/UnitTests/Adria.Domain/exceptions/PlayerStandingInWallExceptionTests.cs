using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class PlayerStandingInWallExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        Random rdm = new();
        // Arrange
        var gameId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var xCord = rdm.NextSingle();
        var yCord = rdm.NextSingle();
            
        // Act
        var exception = new PlayerStandingInWallException(gameId, userId, xCord, yCord);

        // Assert
        Assert.Equal($"User {userId} is claiming to be standing on a wall in game {gameId} at coordinates ({xCord}{yCord})",exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        Random rdm = new();
        // Arrange
        var gameId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var xCord = rdm.NextSingle();
        var yCord = rdm.NextSingle();
        var innerException = new Exception("inner exception");
            
        // Act
        var exception = new PlayerStandingInWallException(gameId, userId, xCord, yCord, innerException);

        // Assert
        Assert.Equal($"User {userId} is claiming to be standing on a wall in game {gameId} at coordinates ({xCord}{yCord})",exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
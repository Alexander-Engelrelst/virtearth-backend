using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class PlayerAlreadyPlayingExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var exception = new PlayerAlreadyPlayingException(id);

        // Assert
        Assert.Equal($"username {id} already playing a game.",exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var innerException = new Exception("Inner exception");
        
        // Act
        var exception = new PlayerAlreadyPlayingException(id, innerException);
        
        // Assert
        Assert.Equal($"username {id} already playing a game.",exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}
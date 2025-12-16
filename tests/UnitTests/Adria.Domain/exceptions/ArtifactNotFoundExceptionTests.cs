using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class ArtifactNotFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var artifactId = Guid.NewGuid();
        
        // Act
        var exception = new ArtifactNotFoundException(userId, artifactId, gameId);

        // Assert
        Assert.Equal(
            $"User {userId} is trying to find a non-existing artifact {artifactId} in {gameId}", exception.Message
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
        var artifactId = Guid.NewGuid();
        
        // Act
        var exception = new ArtifactNotFoundException(userId, artifactId, gameId, innerException);

        // Assert
        Assert.Equal(
            $"User {userId} is trying to find a non-existing artifact {artifactId} in {gameId}", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}
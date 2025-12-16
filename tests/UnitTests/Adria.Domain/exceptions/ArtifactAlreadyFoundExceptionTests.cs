using Adria.Domain.Shared;

namespace UnitTests.Adria.Domain.exceptions;

public class ArtifactAlreadyFoundExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_CreatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var artifactId = Guid.NewGuid();
        
        // Act
        var exception = new ArtifactAlreadyFoundException(userId, artifactId);

        // Assert
        Assert.Equal(
            $"User {userId} already found artifact {artifactId}", exception.Message
        );
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_CreatesException()
    {
        // Arrange
        var innerException = new Exception("Inner exception");

        var userId = Guid.NewGuid();
        var artifactId = Guid.NewGuid();
        
        // Act
        var exception = new ArtifactAlreadyFoundException(userId, artifactId, innerException);

        // Assert
        Assert.Equal(
            $"User {userId} already found artifact {artifactId}", exception.Message
        );
        Assert.Equal(innerException, exception.InnerException);
    }
}
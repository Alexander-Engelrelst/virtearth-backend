using Adria.Domain.games;

namespace UnitTests.Adria.Domain;

public class MazeArtifactTests
{
    /*public MazeArtifact(Guid id, string name, string description)
    {
        if (id == Guid.Empty) throw new ArgumentException("id cannot be empty");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name cannot be empty");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("description cannot be empty");
        
        Id = id;
        Name = name;
        Description = description;
    }*/

    [Fact]
    public void EmptyGuidThrows()
    {
        var exception = Assert.Throws<ArgumentException>(() => new MazeArtifact(Guid.Empty, "name", "description"));
        Assert.Equal("id cannot be empty", exception.Message);
    }

    [Fact]
    public void EmptyNameThrows()
    {
        var exception = Assert.Throws<ArgumentException>(() => new MazeArtifact(Guid.NewGuid(), string.Empty, "description"));
        Assert.Equal("name cannot be empty", exception.Message);
    }

    [Fact]
    public void WhiteSpaceNameThrows()
    {
        var exception = Assert.Throws<ArgumentException>(() => new MazeArtifact(Guid.NewGuid(), "   ", "description"));
        Assert.Equal("name cannot be empty", exception.Message);
    }

    [Fact]
    public void EmptyDescriptionThrows()
    {
        var exception = Assert.Throws<ArgumentException>(() => new MazeArtifact(Guid.NewGuid(), "name", string.Empty));
        Assert.Equal("description cannot be empty", exception.Message);
    }

    [Fact]
    public void WhiteSpaceDescriptionThrows()
    {
        var exception = Assert.Throws<ArgumentException>(() => new MazeArtifact(Guid.NewGuid(), "name", "   "));
        Assert.Equal("description cannot be empty", exception.Message);
    }

    [Fact]
    public void EqualityWorks()
    {
        Guid id = Guid.NewGuid();
        MazeArtifact artifact1 = new MazeArtifact(id, "name1", "description1");
        MazeArtifact artifact2 = new MazeArtifact(id, "name2", "description2");
        Assert.Equal(artifact1, artifact2);
    }
}
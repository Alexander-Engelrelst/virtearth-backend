namespace Adria.Domain.games;

public class MazeArtifact : MazeElement
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    
    public MazeArtifact(Guid id, string name, string description)
    {
        // TODO write tests for this
        if (id == Guid.Empty) throw new ArgumentException("id cannot be empty");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name cannot be empty");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("description cannot be empty");
        
        Id = id;
        Name = name;
        Description = description;
    }
}
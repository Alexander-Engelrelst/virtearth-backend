namespace Adria.Domain.games;

public class MazeArtifact : MazeElement
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    
    public MazeArtifact(Guid id, string name, string description)
    {
        if (id == Guid.Empty) throw new ArgumentException("id cannot be empty", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("description cannot be empty",  nameof(description));
        
        Id = id;
        Name = name;
        Description = description;
    }

    protected bool Equals(MazeArtifact other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MazeArtifact)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
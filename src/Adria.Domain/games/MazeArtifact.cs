namespace Adria.Domain.games;

public sealed class MazeArtifact : IMazeElement, IEquatable<MazeArtifact>
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

    public bool Equals(MazeArtifact? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is MazeArtifact other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
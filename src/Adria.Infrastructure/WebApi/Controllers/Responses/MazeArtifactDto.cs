using Adria.Domain.games;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed class MazeArtifactDto
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public int X { get; }
    public int Y { get; }
    
    public MazeArtifactDto(MazeArtifact artifact, int xCord, int yCord)
    {
        Id = artifact.Id;
        Name = artifact.Name;
        Description = artifact.Description;
        X = xCord;
        Y = yCord;
    }


    private bool Equals(MazeArtifactDto other)
    {
        return Id.Equals(other.Id) && Name == other.Name && Description == other.Description && X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MazeArtifactDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Description, X, Y);
    }
}
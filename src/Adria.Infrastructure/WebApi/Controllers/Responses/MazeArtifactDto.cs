using Adria.Domain.games;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public class MazeArtifactDto
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
    
}
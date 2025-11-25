namespace Adria.Domain.games;

public class MazeArtifact(Guid id, string name, string description) : MazeElement
{
    private Guid Id { get; } = id;
    private string Name { get; } = name;
    private string Description { get; } = description;
}
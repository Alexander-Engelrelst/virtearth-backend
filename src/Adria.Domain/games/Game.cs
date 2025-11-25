namespace Adria.Domain.games;

public enum GameTypes
{
    Maze
}
public abstract class Game(Guid userId, Guid gameId)
{
    private Guid UserId { get; init; } = userId;
    private Guid GameId { get; init; } = gameId;
}
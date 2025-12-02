namespace Adria.Domain.games;

public abstract class Game(Guid userId, Guid gameId)
{
    // TODO ensure the constructor throws an error for invalid ids
    public Guid UserId { get; } = userId;
    public Guid GameId { get; } = gameId;
}
namespace Adria.Domain.games;

public abstract class Game(Guid userId, Guid gameId)
{
    public Guid UserId { get; } = userId;
    public Guid GameId { get; } = gameId;
}
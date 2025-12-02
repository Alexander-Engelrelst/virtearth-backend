namespace Adria.Domain.games;

public abstract class Game
{
    // TODO ensure the constructor throws an error for invalid ids
    public Guid UserId { get; }
    public Guid GameId { get; }

    protected Game(Guid userId, Guid gameId)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("gameId cannot be empty");
        if (userId == Guid.Empty) throw new ArgumentException("userId cannot be empty");
        
        UserId = userId;
        GameId = gameId;
    }
}
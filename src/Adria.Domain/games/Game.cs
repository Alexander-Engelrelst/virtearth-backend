namespace Adria.Domain.games;

public abstract class Game
{
    public Guid UserId { get; }
    public Guid GameId { get; }

    protected Game(Guid userId, Guid gameId)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("gameId cannot be empty", nameof(gameId));
        if (userId == Guid.Empty) throw new ArgumentException("userId cannot be empty", nameof(userId));
        
        UserId = userId;
        GameId = gameId;
    }

    protected bool Equals(Game other)
    {
        return UserId.Equals(other.UserId) && GameId.Equals(other.GameId);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Game)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, GameId);
    }
}
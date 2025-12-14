using Adria.Domain.Users;

namespace Adria.Domain.games;

#pragma warning disable S4035
/* for some reason sonar was being 'very smart' saying I needed to implement an IEqualityComparer when I use IEquatable
 * I was not using IEquatable but do know after looking it up, the behaviour is still intentional and adding new games
 * won't break the comparison because I believe it is quite clear that 2 games are the same if they have the same Guid */
public abstract class Game : IEquatable<Game>
{
    public User User { get; }
    public Guid GameId { get; }

    protected Game(Guid gameId, User user)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("gameId cannot be empty", nameof(gameId));
        
        User = user;
        GameId = gameId;
    }

    public bool Equals(Game? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return User.Equals(other.User) && GameId.Equals(other.GameId);
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
        return HashCode.Combine(User, GameId);
    }

    public abstract bool IsFinished();
}

#pragma warning restore S4035
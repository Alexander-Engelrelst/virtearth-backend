namespace Adria.Domain.games;

/* Normally you would be using inheritance for each different type of game,
 * Since we will only be implementing one for our POC I do not feel the need for this added complexity */
public abstract class Game(Guid userId, Guid gameId)
{
    private Guid UserId { get; init; } = userId;
    private Guid GameId { get; init; } = gameId;
}
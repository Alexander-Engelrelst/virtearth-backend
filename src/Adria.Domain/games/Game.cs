namespace Adria.Domain.games;

/* I am aware this looks overengineered and it kind of is but I was told the code had to be good,
 * this is what i would do for a prod server that had multiple games
 * only for a server with only one fully implemented game this looks very stupid ¯\_(ツ)_/¯ */
public enum GameTypes
{
    Maze
}
public abstract class Game(Guid userId, Guid gameId)
{
    private Guid UserId { get; init; } = userId;
    private Guid GameId { get; init; } = gameId;
}
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Adria.Domain.Shared;

namespace Adria.Domain.games;

public static class ActiveGames
{
    private static readonly ConcurrentDictionary<Guid, Game> _games = new();
    public static TimeSpan GAME_TTL { get; } = TimeSpan.FromSeconds(15);
    public static void AddGame(Game game)
    {
        if (!_games.TryAdd(game.User.Id, game))
        {
            throw new PlayerAlreadyPlayingException(game.User.Id);
        }
    }

    public static Game Get(Guid userId, Guid expectedGameId, bool mustBeFinished = false)
    {
        if (!_games.TryGetValue(userId, out Game? game))
        {
            throw new ActiveGameNotFoundException(userId);
        }
        
        if (game.GameId != expectedGameId)
        {
            throw new GameIdMismatchException(userId);
        }

        if (mustBeFinished && !game.IsFinished())
        {
            throw new GameNotFinishedException(game.GameId, userId);
        }
        
        return game;
    }

    public static void RemoveUnplayedGames()
    {
        foreach (var game in _games.Values)
        {
            if (game.TimeOfLastHeartBeat.Add(GAME_TTL) < DateTime.UtcNow)
            {
                _games.TryRemove(game.User.Id, out _);
            }
        }
    }
}
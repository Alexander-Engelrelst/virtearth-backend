using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Adria.Domain.Shared;

namespace Adria.Domain.games;

public static class ActiveGames
{
    public static readonly Dictionary<Guid, Game> _games = new();

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
        
        // if mustBeFinished is true that means this is used to save to game and so it must be deleted from the cache
        // TODO add a heartbeat route instead of this
        if (mustBeFinished) Remove(game.User.Id);
        
        return game;
    }

    public static void Remove(Guid userId)
    {
        _games.Remove(userId);
    }
}
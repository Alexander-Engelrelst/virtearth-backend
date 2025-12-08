using System.Collections.ObjectModel;
using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain.games;

public static class ActiveGames
{
    private static readonly Dictionary<Guid, Game> _games = new();
    public static ReadOnlyDictionary<Guid, Game> Games => new(_games);

    public static void AddGame(Game game)
    {
        if (!_games.TryAdd(game.UserId, game))
        {
            throw new PlayerAlreadyPlayingException(game.UserId);
        }
    }

    public static Game Get(Guid userId)
    {
        if (!_games.TryGetValue(userId, out Game? game))
        {
            throw new ElementNotFoundException($"User {userId} is currently not playing a game");
        }

        return game;
    }
}
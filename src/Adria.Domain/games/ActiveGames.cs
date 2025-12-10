using System.Collections.ObjectModel;
using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain.games;

public static class ActiveGames
{
    private static readonly Dictionary<Guid, Game> _games = new();

    public static void AddGame(Game game)
    {
        if (!_games.TryAdd(game.User.Id, game))
        {
            throw new PlayerAlreadyPlayingException(game.User.Id);
        }
    }

    public static Game Get(Guid userId)
    {
        if (!_games.TryGetValue(userId, out Game? game))
        {
            throw new ActiveGameNotFoundException(userId);
        }

        return game;
    }
}
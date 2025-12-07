namespace Adria.Domain.games;

public static class ActiveGames
{
    private static List<Game> _games = [];
    public static IList<Game> Games => _games.AsReadOnly();

    public static void AddGame(Game game)
    {
        _games.Add(game);
    }
}
namespace Adria.Domain.games;

public static class ActiveGames
{
    private static IList<Game> _games = new List<Game>();
    public static IList<Game> Games => _games.AsReadOnly();

    public static void AddGame(Game game)
    {
        _games.Add(game);
    }
}
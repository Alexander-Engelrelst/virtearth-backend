namespace Adria.Domain.games;

public static class ActiveGames
{
    private static IList<Game> Games { get; } = new List<Game>();

    public static IList<Game> GetGames()
    {
        return Games.AsReadOnly();
    }

    public static void AddGame(Game game)
    {
        Games.Add(game);
    }
}
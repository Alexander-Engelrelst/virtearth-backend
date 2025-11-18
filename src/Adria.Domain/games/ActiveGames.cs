namespace Adria.Domain.games;

public class ActiveGames
{
    private static IList<Game> Games { get; } = new List<Game>();

    public static IList<Game> GetGames()
    {
        return Games.AsReadOnly();
    }

    public static void AddGame()
    {
        // TODO implement this
    }
}
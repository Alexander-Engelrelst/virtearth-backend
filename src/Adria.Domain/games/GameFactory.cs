using Adria.Application.Contracts;

namespace Adria.Domain.games;

// this feels extremely overengineered for a single game in the database
public class GameFactory : IGameFactory
{
    public MazeGame Create(MazeGameData data)
    {
        return new MazeGame(data.GameId, data.UserId, data.Artifacts);
    }
}
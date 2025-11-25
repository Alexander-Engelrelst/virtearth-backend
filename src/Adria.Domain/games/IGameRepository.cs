namespace Adria.Domain.games;

public interface IGameRepository
{
    Task<GameTypes> GetGameType(Guid id);
    MazeGameData GetMazeGameData(Guid id);
}
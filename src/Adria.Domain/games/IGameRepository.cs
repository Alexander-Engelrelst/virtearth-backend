namespace Adria.Domain.games;

public interface IGameRepository
{
    MazeGameData GetMazeGameData(Guid id);
}
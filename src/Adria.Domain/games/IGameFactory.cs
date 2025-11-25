using Adria.Domain.games;

namespace Adria.Application.Contracts;

public interface IGameFactory
{
    MazeGame Create(MazeGameData data);
}
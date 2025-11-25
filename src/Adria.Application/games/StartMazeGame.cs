using Adria.Application.Contracts;
using Adria.Domain.games;

namespace Adria.Application.games;

public class StartMazeGame : IUseCase<Guid,MazeGame>
{
    public MazeGame Execute(Guid id)
    {
        throw new NotImplementedException();
    }
}
namespace Adria.Domain.games;

public interface IGameRepository
{
    Task Save(Game game);
}
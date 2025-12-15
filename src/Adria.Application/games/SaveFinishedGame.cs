using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.games;

public sealed record SaveFinishedGameInput(User User, Guid GameId);

public class SaveFinishedGame : IUseCase<SaveFinishedGameInput>
{
    private readonly ILogger<SaveFinishedGame> _logger;
    private readonly IGameRepository  _gameRepository;
    public SaveFinishedGame(
        ILogger<SaveFinishedGame> logger,
        IGameRepository gameRepository
    )
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }
    
    
    public async Task Execute(SaveFinishedGameInput input)
    {
        Game game = ActiveGames.Get(input.User.Id, input.GameId, true);
        
        await _gameRepository.Save(game);
    }
}
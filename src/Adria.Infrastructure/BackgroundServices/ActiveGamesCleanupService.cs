using Adria.Domain.games;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.BackgroundServices;

/* this class is based on documentation found at
 * https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/background-tasks-with-ihostedservice*/
public sealed class ActiveGamesCleanupService : BackgroundService
{
    private readonly ILogger<ActiveGamesCleanupService> _logger;

    public ActiveGamesCleanupService(ILogger<ActiveGamesCleanupService> logger)
    {
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Cache Cleanup Service is starting.");
        
        stoppingToken.Register(() => _logger.LogInformation("Cache Cleanup Service is stopping."));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(ActiveGames.GAME_TTL, stoppingToken);
            ActiveGames.RemoveUnplayedGames();
        }
    }
}
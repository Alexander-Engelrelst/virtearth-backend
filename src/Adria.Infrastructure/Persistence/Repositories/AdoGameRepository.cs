using System.Data.Common;
using Adria.Domain.games;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoGameRepository : AbstractAdoRepository, IGameRepository
{
    private ILogger<AdoGameRepository> _logger;
    private const string TABLE_COMPLETED_GAMES = "completed_games";
    private const string COL_COMPLETED_GAMES_GAME_ID = "game_id";
    private const string COL_COMPLETED_GAMES_USER_ID = "user_id";

    private const string INSERT_COMPLETED_GAME = $@"
        INSERT INTO {TABLE_COMPLETED_GAMES} ({COL_COMPLETED_GAMES_USER_ID}, {COL_COMPLETED_GAMES_GAME_ID})
        VALUES (@userId, @gameId);
    ";
    
    public AdoGameRepository(DbProviderFactory factory, string connectionString, ILogger<AdoGameRepository> logger) 
        : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task Save(Game game)
    {
        _logger.LogInformation("Saving completed game {GameId} for user {UserId}", game.GameId, game.User.Id);

        try
        {
            await ExecuteNonQueryAsync(
                INSERT_COMPLETED_GAME,
                [CreateParameter("@userId", game.User.Id), CreateParameter("@gameId", game.GameId)]);
        }
        catch (DuplicatePrimaryKeyException ex)
        {
            _logger.LogWarning(ex, "Duplicate key values for game {GameId} and {UserId}", game.GameId, game.User.Id);
            throw new GameAlreadyCompletedByUserException(game.GameId, game.User.Id, ex);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save Finished game {GameId} for user {UserId}", game.GameId, game.User.Id);
            throw new VirtEarthDatabaseException("Failed to save finished game to database.", ex);
        }
    }
}
using System.Data.Common;
using Adria.Domain.games;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoGameRepository : AbstractAdoRepository, IGameRepository
{
    private readonly ILogger<AdoGameRepository> _logger;
    private const string COLUMN_TYPE = "type";
    private const string TABLE_GAMES = "games";
    
    private const string GAME_TYPE_QUERY = $@"
        select {COLUMN_TYPE}
        from {TABLE_GAMES}
        where `id` = @id
";
    public AdoGameRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoGameRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<GameTypes> GetGameType(Guid id)
    {
        _logger.LogInformation("Getting game type for id {id}", id);
        
        DbParameter idParameter = CreateParameter("@Id", id.ToString().ToLower());
        string? result = await ExecuteScalarAsync(GAME_TYPE_QUERY, [idParameter]) as string;
        if (!Enum.TryParse<GameTypes>(result, out var gameType))
        {
            _logger.LogError("game with id {id} has an invalid gametype in the database", id);
            throw new VirtEarthDatabaseException("The game can not be started");
        }

        return gameType;
    }

    public MazeGameData GetMazeGameData(Guid id)
    {
        throw new NotImplementedException();
    }
}
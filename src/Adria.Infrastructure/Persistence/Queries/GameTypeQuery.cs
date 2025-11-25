using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using ZstdSharp.Unsafe;

namespace Adria.Infrastructure.Persistence.Queries;

public class GameTypeQuery : IGameTypeQuery
{
    private const string QRY = $@"
        select type
        from games
        where `id` = @id
";
    
    private readonly ILogger _logger;
    private readonly string _connectionString;
    private readonly DbProviderFactory _factory;
    public GameTypeQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<GameTypeQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    
    public async Task<GameTypes> Fetch(Guid id)
    {
        _logger.LogInformation("Getting game type for id {Id}.", id);
        
        using var connection = _factory.CreateConnection() 
            ?? throw new InvalidOperationException("Could not create connection to db.");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@id";
        parameter.Value = id;
        
        command.Parameters.Add(parameter);

        string? typeAsString = await command.ExecuteScalarAsync() as string;
        if (typeAsString is null)
        {
            _logger.LogWarning("No game type found in the database for game with id {Id}.", id);
            throw new VirtEarthDatabaseException($"No game type found for the game with id {id}.");
        }
        
        if (!Enum.TryParse<GameTypes>(typeAsString, true, out var gameType))
        {
            _logger.LogError("Game with id {Id} has an invalid gametype '{GameType}' in the database.", id, typeAsString);
            throw new VirtEarthDatabaseException($"The game type '{typeAsString}' in the database doesn't exist.");
        }

        return gameType;

    }
    
}
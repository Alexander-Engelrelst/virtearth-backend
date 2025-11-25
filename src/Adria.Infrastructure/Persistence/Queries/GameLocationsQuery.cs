using System.Collections.ObjectModel;
using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class GameLocationsQuery : IGameLocationsQuery
{

    private const string QRY = @"
    SELECT `id`, `name`, `latitute`, `longitude`
    FROM `games`";
    
    private readonly ILogger _logger;
    private readonly string _connectionString;
    private readonly DbProviderFactory _factory;
    public GameLocationsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<GameLocationsQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    public async Task<ReadOnlyCollection<GameLocation>> Fetch()
    {
        _logger.LogInformation("Fetching all games");
        
        using var connection = _factory.CreateConnection()
                                        ?? throw new InvalidOperationException(
                                            "DbProviderFactory returned a null DbConnection.");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
    
        command.CommandText = QRY;

        var games = new List<GameLocation>();

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var nameOrd = reader.GetOrdinal("name");
            var latituteOrd = reader.GetOrdinal("latitute");
            var longitudeOrd = reader.GetOrdinal("longitude");
            
            // TODO also insert a year and continent property to a game
            games.Add(new GameLocation(
                reader.GetGuid(idOrd),
                reader.GetString(nameOrd),
                reader.GetDouble(latituteOrd),
                reader.GetDouble(longitudeOrd)
                ));
        }

        _logger.LogInformation("Fetched {Count} games locations", games.Count);

        return games.AsReadOnly();
    }
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class GameLocationsQuery : IGameLocationsQuery
{

    private const string QRY = @"
    SELECT `id`, `name`, `latitute`, `longitude`, `continent`, `year`
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
    public async Task<IReadOnlyCollection<GameLocation>> Fetch()
    {
        _logger.LogInformation("Fetching all games");

        await using var connection = _factory.CreateConnection()
                                     ?? throw new InvalidOperationException(
                                         "DbProviderFactory returned a null DbConnection.");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
    
        command.CommandText = QRY;

        var games = new List<GameLocation>();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var nameOrd = reader.GetOrdinal("name");
            var latituteOrd = reader.GetOrdinal("latitute");
            var longitudeOrd = reader.GetOrdinal("longitude");
            var continentOrd = reader.GetOrdinal("continent");
            var yearOrd = reader.GetOrdinal("year");
            
            string continentAsString = reader.GetString(continentOrd);
            if (!Enum.TryParse<Continent>(continentAsString, out Continent continent))
            {
                _logger.LogCritical(
                    "Continent {Continent} doesn't exist but is in the database for id {Id}",
                    continentAsString,
                    reader.GetGuid(idOrd));
                
                throw new InvalidEnumArgumentException(
                    argumentName: nameof(continentAsString),
                    invalidValue: -1,                 // must be an int!  
                    enumClass: typeof(Continent)
                );
            }
            
            games.Add(new GameLocation(
                reader.GetGuid(idOrd),
                reader.GetString(nameOrd),
                reader.GetDouble(latituteOrd),
                reader.GetDouble(longitudeOrd),
                continent,
                reader.GetInt32(yearOrd)
                ));
        }

        _logger.LogInformation("Fetched {Count} games locations", games.Count);

        return games.AsReadOnly();
    }
}
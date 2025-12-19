using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class GameLocationsQuery : IGameLocationsQuery
{

    private const string QRY = @"
    SELECT g.id, g.name, g.latitude, g.longitude, g.continent, g.year, g.description,cg.user_id
    FROM `games` AS g
    LEFT JOIN `completed_games` AS cg ON g.id = cg.game_id AND cg.user_id = @userId";
    
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
    public async Task<IReadOnlyCollection<GameLocation>> Fetch(Guid userId)
    {
        _logger.LogInformation("Fetching all games");

        await using DbConnection connection = _factory.CreateConnection() ?? 
                                     throw new VirtEarthDatabaseException("Failed to create a database connection.");

        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        await using DbCommand command = connection.CreateCommand();
    
        command.CommandText = QRY;
        
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = "@userId";
        parameter.Value = userId;
        command.Parameters.Add(parameter);
        
        List<GameLocation> games = [];

        await using DbDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            int idOrd = reader.GetOrdinal("id");
            int nameOrd = reader.GetOrdinal("name");
            int latituteOrd = reader.GetOrdinal("latitude");
            int longitudeOrd = reader.GetOrdinal("longitude");
            int continentOrd = reader.GetOrdinal("continent");
            int yearOrd = reader.GetOrdinal("year");
            int descriptionOrd = reader.GetOrdinal("description");
            int userIdOrd = reader.GetOrdinal("user_id");
            
            string continentAsString = reader.GetString(continentOrd);
            if (!Enum.TryParse(continentAsString, out Continent continent))
            {
                _logger.LogCritical(
                    "Continent {Continent} doesn't exist but is in the database for id {Id}",
                    continentAsString,
                    reader.GetGuid(idOrd));
                
                throw new InvalidEnumArgumentException(
                    argumentName: nameof(continentAsString),
                    invalidValue: -1,
                    enumClass: typeof(Continent)
                );
            }
            
            games.Add(new GameLocation(
                reader.GetGuid(idOrd),
                reader.GetString(nameOrd),
                reader.GetDouble(latituteOrd),
                reader.GetDouble(longitudeOrd),
                continent,
                reader.GetInt32(yearOrd),
                reader.GetString(descriptionOrd),
                !await reader.IsDBNullAsync(userIdOrd)
                ));
        }

        _logger.LogInformation("Fetched {Count} games locations", games.Count);

        return games.AsReadOnly();
    }
}
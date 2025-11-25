using System.Collections.ObjectModel;
using System.Data.Common;
using System.Runtime.InteropServices.JavaScript;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class ArtifactsQuery : IArtifactsQuery
{
    private const string QRY = @"
    SELECT `id`, `name`, `description`
    FROM `artifacts`
    WHERE `game_id` = @id";
    
    private readonly ILogger<ArtifactsQuery> _logger;
    private readonly string _connectionString;
    private readonly DbProviderFactory _factory;
    public ArtifactsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<ArtifactsQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task<ReadOnlyCollection<MazeArtifact>> Fetch(Guid id)
    {
        Console.WriteLine(id);
        _logger.LogInformation("Fetching all artifacts for game {Id}", id);

        await using var connection = _factory.CreateConnection()
                                     ?? throw new InvalidOperationException(
                                         "DbProviderFactory returned a null DbConnection.");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
    
        command.CommandText = QRY;
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@id";
        parameter.Value = id.ToString();
        command.Parameters.Add(parameter);
        
        var artifacts = new List<MazeArtifact>();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var nameOrd = reader.GetOrdinal("name");
            var descriptionOrd = reader.GetOrdinal("description");
            
            artifacts.Add(new MazeArtifact(
                reader.GetGuid(idOrd),
                reader.GetString(nameOrd),
                reader.GetString(descriptionOrd)
            ));
        }

        _logger.LogInformation("Fetched {Count} artifacts", artifacts.Count);

        return artifacts.AsReadOnly();
    }
}
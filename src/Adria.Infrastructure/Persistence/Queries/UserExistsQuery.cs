using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class UserExistsQuery : IUserExistsQuery
{
    private const string QRY = @"
        SELECT EXISTS (
            SELECT 1 FROM users WHERE username = @username
        ) as UserNameExists;
    ";

    private readonly ILogger _logger;
    private readonly string _connectionString;
    private readonly DbProviderFactory _factory;
    public UserExistsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<UserExistsQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    public async Task<bool> Fetch(string username)
    {
        _logger.LogInformation("Checking if user with username {Username} exists", username);

        await using DbConnection connection = _factory.CreateConnection() ?? 
                                     throw new VirtEarthDatabaseException("Failed to create a database connection..");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        await using DbCommand command = connection.CreateCommand();
    
        command.CommandText = QRY;
            
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = "@username";
        parameter.Value = username;
        
        command.Parameters.Add(parameter);
        
        bool exists =  Convert.ToBoolean(await command.ExecuteScalarAsync());
        
        if (exists) _logger.LogInformation("Query finished: {Username} already exists", username);
        
        return exists;
    }
}
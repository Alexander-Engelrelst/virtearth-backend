using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.Users;
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
        _logger.LogInformation("Checking if user with username {username} exists", username);

        using var connection = _factory.CreateConnection() 
                               ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = QRY;
                
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@username";
            parameter.Value = username;
            
            _logger.LogInformation("Query finished: {username} already exists", username);
            
            return Convert.ToBoolean(await command.ExecuteScalarAsync());
        }
    }
}
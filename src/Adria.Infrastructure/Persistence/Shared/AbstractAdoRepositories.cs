using System.Data.Common;

namespace Adria.Infrastructure.Persistence.Shared;

public abstract class AbstractAdoRepository
{
    protected readonly DbProviderFactory _factory;
    protected readonly string _connectionString;

    protected AbstractAdoRepository(
        DbProviderFactory factory,
        string connectionString
    )
    {
        _factory = factory;
        _connectionString = connectionString;
    }

    protected DbParameter CreateParameter(string name, object value)
    {
        DbParameter parameter = _factory.CreateParameter()!;
        parameter.ParameterName = name;
        parameter.Value = value;
        return parameter;
    }

    protected async Task ExecuteNonQueryAsync(string commandText, DbParameter[] parameters)
    {
        try
        {
            DbConnection connection = await OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddRange(parameters);
            await command.ExecuteNonQueryAsync();
        }
        catch (DbException ex)
        {
            throw new VirtEarthDatabaseException("Database operation failed.", ex);
        }
        catch (ArgumentException ex)
        {
            throw new VirtEarthDatabaseException("Invalid argument for database operation.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new VirtEarthDatabaseException("Invalid operation during database access.", ex);
        }
    }

    protected async Task<DbDataReader> ExecuteReaderAsync(string commandText, DbParameter[] parameters)
    {
        try
        {
            var connection = await OpenConnection();
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddRange(parameters);
            return await command.ExecuteReaderAsync();
        }
        catch (DbException ex)
        {
            throw new VirtEarthDatabaseException("Database operation failed.", ex);
        }
        catch (ArgumentException ex)
        {
            throw new VirtEarthDatabaseException("Invalid argument for database operation.", ex);
        }
        catch (InvalidOperationException ex)
        {
            throw new VirtEarthDatabaseException("Invalid operation during database access.", ex);
        }
    }

    protected async Task<DbConnection> OpenConnection()
    {
        var connection = _factory.CreateConnection() ??
             throw new InvalidOperationException("Failed to create a database connection.");

        connection.ConnectionString = _connectionString;

        await connection.OpenAsync();

        return connection;
    }
}
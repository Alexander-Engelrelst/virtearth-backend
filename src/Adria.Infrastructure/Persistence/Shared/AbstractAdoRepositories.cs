using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Adria.Infrastructure.Persistence.Shared;

public abstract class AbstractAdoRepository
{
    private const int MYSQL_DUPLICATE_ENTRY_STATUS_CODE = 1062;

    
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

    protected DbParameter CreateParameter(string name, object? value)
    {
        DbParameter parameter = _factory.CreateParameter()!;
        parameter.ParameterName = name;
        parameter.Value = value ??  DBNull.Value;
        return parameter;
    }

    protected async Task ExecuteNonQueryAsync(string commandText, DbParameter[] parameters)
    {
        try
        {
            await using DbConnection connection = await OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddRange(parameters);
            await command.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.Number == MYSQL_DUPLICATE_ENTRY_STATUS_CODE)
        {
            throw new DuplicatePrimaryKeyException(ex.Message, ex);
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
            return await command.ExecuteReaderAsync(
                CommandBehavior.CloseConnection //this is needed to ensure the connection gets closed when the reader is finished
                );
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
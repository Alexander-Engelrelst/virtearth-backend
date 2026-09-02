using System.Data.Common;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoUserRepository : AbstractAdoRepository, IUserRepository
{
    private readonly ILogger<AdoUserRepository> _logger;

    private const string TABLE_USERS = "users";
    private const string COL_ID = "id";
    private const string COL_NAME = "username";

    private const string INSERT_USER = $@"
        INSERT INTO {TABLE_USERS} ({COL_ID}, {COL_NAME})
        VALUES (@Id, @Name);
    ";

    private const string SELECT_USER_BY_ID = $@"
        SELECT {COL_ID}, {COL_NAME}
        FROM {TABLE_USERS}
        WHERE {COL_ID} = @Id;
    ";
    
    private static readonly string UPDATE_USER = $@"
        UPDATE {TABLE_USERS}
        SET {COL_NAME} = @Name
        WHERE {COL_ID} = @Id;
    ";

    public AdoUserRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoUserRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task Save(User user)
    {
        _logger.LogInformation("Saving user with ID {UserId} to database.", user.Id);
        string userQuery = INSERT_USER;

        if (await ById(user.Id) != null)
        {
            userQuery = UPDATE_USER;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", user.Id.ToString().ToLower()),
                CreateParameter("@Name", user.Username),
            ];

            await ExecuteNonQueryAsync(userQuery, parameters);

        }
        /* this will catch it if for some reason the client manages to send a username that is already in the database
           The reason we don't precheck this is because this way we avoid any race conditions causing issues
           since race conditions can still occur when we add try to do it this way
           the name still should get prechecked in the client to render a correct error message before saving */
        catch (DuplicatePrimaryKeyException ex)
        {
            _logger.LogError(ex, "Failed to save user with ID {UserId} to database.", user.Id);
            throw new UsernameAlreadyExistsException(user.Username, ex);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save user with ID {UserId} to database.", user.Id);
            throw new VirtEarthDatabaseException("Failed to save user to database.", ex);
        }
    }

    public async Task<User?> ById(Guid userId)
    {
        DbParameter id = CreateParameter("@Id", userId.ToString().ToLower()); 
        await using DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_USER_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new User(
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID))
                );
            }

            return null;
        }
        
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read user with ID {UserId} from database.", userId);
            throw new VirtEarthDatabaseException("Failed to read user from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for user with ID {UserId}.", userId);
        }
    }
}
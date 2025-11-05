using System.Data.Common;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoUserRepository : AbstractAdoRepository, IUserRepository
{
    private readonly ILogger<AdoUserRepository> _logger;

    private const string TABLE_USERS = "users";
    private const string COL_ID = "id";
    private const string COL_NAME = "username";
    private const string COL_AVATAR = "avatar";

    private const string INSERT_USER = $@"
        INSERT INTO {TABLE_USERS} ({COL_ID}, {COL_NAME}, {COL_AVATAR})
        VALUES (@Id, @Name, @Avatar);
    ";

    private const string SELECT_USER_BY_ID = $@"
        SELECT {COL_ID}, {COL_NAME}, {COL_AVATAR}
        FROM {TABLE_USERS}
        WHERE {COL_ID} = @Id;
    ";
    
    private static readonly string UPDATE_USER = $@"
        UPDATE {TABLE_USERS}
        SET {COL_NAME} = @Name, {COL_AVATAR}  = @Avatar
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

        if ((await ById(user.Id)) != null)
        {
            userQuery = UPDATE_USER;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", user.Id.ToString().ToLower()),
                CreateParameter("@Name", user.Username),
                CreateParameter("@Avatar", user.Avatar?.ToString() ?? null)
            ];

            await ExecuteNonQueryAsync(userQuery, parameters);

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
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_USER_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                string avatarString = dbDataReader.GetString(dbDataReader.GetOrdinal(COL_AVATAR));

                Avatar? avatar = Enum.TryParse<Avatar>(avatarString, true ,out Avatar result)
                    ? result
                    : null;
                
                return new User(
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    avatar,
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
            await dbDataReader.DisposeAsync();
        }
    }
}
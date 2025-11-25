using System.Configuration;
using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.games;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Queries;
using Adria.Infrastructure.Persistence.Repositories;

namespace Adria.Main.Modules.Persistence;

public static class PersistenceModule
{
    private static string _connectionString = string.Empty;
    
    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _connectionString = configuration["Persistence:ConnectionString"]
                            ?? throw new ConfigurationErrorsException("Persistence:ConnectionString was not found");
        
        return services
            .AddAdoServices(configuration)
            .AddRepositories()
            .AddQueries();
    }

    public static WebApplication UsePersistenceModule(this WebApplication app)
    {
        return app.ApplyMigrations();
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        // Configure repositories here.
        return services.AddScoped<IUserRepository, AdoUserRepository>(serviceProvider =>
        {
            return new AdoUserRepository(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AdoUserRepository>>()
            );
        })
        .AddScoped<IGameRepository, AdoGameRepository>(serviceProvider =>
        {
            return new AdoGameRepository(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<AdoGameRepository>>()
            );
        });
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        // Configure queries here.
        return services.AddScoped<IUserExistsQuery, UserExistsQuery>(serviceProvider =>
        {
            return new UserExistsQuery(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<UserExistsQuery>>()
            );
        })
        .AddScoped<IGameLocationsQuery, GameLocationsQuery>(serviceProvider =>
        {
            return new GameLocationsQuery(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<GameLocationsQuery>>()
            );
        })
        .AddScoped<IGameTypeQuery, GameTypeQuery>(serviceProvider =>
        {
            return new GameTypeQuery(
                serviceProvider.GetRequiredService<DbProviderFactory>(),
                _connectionString,
                serviceProvider.GetRequiredService<ILogger<GameTypeQuery>>()
            );
        });
    }

    private static IServiceCollection AddAdoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string provider = configuration["Persistence:Provider"]!;

        DbProviderFactories.RegisterFactory(provider, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

        return services.AddScoped(serviceProvider =>
        {
            return DbProviderFactories.GetFactory(provider);
        });
    }
    
    private static WebApplication ApplyMigrations(this WebApplication app)
    {
        IServiceProvider serviceProvider = app.Services.CreateScope().ServiceProvider;
        DbProviderFactory factory = serviceProvider.GetRequiredService<DbProviderFactory>();
        string connectionString = serviceProvider.GetRequiredService<IConfiguration>()["Persistence:ConnectionString"]!;

        using DbConnection connection = factory.CreateConnection()!;
        connection.ConnectionString = connectionString;
        connection.Open();

        string scriptPath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence/Scripts",
            "create_database.sql"
        );

        using DbCommand command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(scriptPath);
        command.ExecuteNonQuery();
        return app;
    }
}

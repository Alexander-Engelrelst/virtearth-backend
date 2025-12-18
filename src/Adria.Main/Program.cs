using Adria.Infrastructure.BackgroundServices;
using Adria.Main.Modules.Authentication;
using Adria.Main.Modules.Persistence;
using Adria.Main.Modules.UseCases;
using Adria.Main.Modules.WebApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder
    .Services
        .AddHostedService<ActiveGamesCleanupService>()
        .AddHttpContextAccessor()
        .AddPersistenceModule(configuration)
        .AddWebApiModule(configuration)
        .AddUseCases()
        .AddJwtAuthentication();

WebApplication app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
    
await app
    .UsePersistenceModule()
    .UseWebApiModule()
    .RunAsync();

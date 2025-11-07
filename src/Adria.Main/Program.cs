using Adria.Main.Modules.Authentication;
using Adria.Main.Modules.Persistence;
using Adria.Main.Modules.UseCases;
using Adria.Main.Modules.WebApi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder
    .Services
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

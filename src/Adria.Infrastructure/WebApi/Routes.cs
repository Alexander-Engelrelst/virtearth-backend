using Adria.Infrastructure.WebApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace Adria.Infrastructure.WebApi;

public static class Routes
{
    private readonly static string APPLICATION_JSON = "application/json";
    
    public static OpenApiInfo OpenApiInfo { get; } = new OpenApiInfo
    {
        Version = "v1",
        Title = "Your nice title here",
        Description = "Your even nicer description here",
        Contact = new OpenApiContact
        {
            Name = "Group XX",
            Email = "info@group-XX.adria"
        }
    };

    public static WebApplication MapRoutes(this WebApplication app)
    {
        MapUserRoutes(app);
        MapGameRoutes(app);
        
        return app;
    }

    private static void MapUserRoutes(WebApplication app)
    {
        RouteGroupBuilder userRoutes = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithDescription("All endpoints related to users")
            .WithOpenApi();
        
        userRoutes
            .MapGet("/exists", CheckUserExistsController.Invoke)
            .WithDescription("Check if a user with a given username exists.")
            .WithName(nameof(CheckUserExistsController))
            .WithOpenApi();

        userRoutes
            .MapPost("/", CreateUserController.Invoke)
            .WithDescription("Register a new username and id")
            .WithName(nameof(CreateUserController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapGet("/login/{id}", LoginController.Invoke)
            .WithDescription("Login a user to get a JWT token")
            .WithName(nameof(LoginController))
            .WithOpenApi();
        
        /* apparently we are not implementing this clientSide for the poc, I thought we were originally
         * but hey shit happens and yes I was to lazy to remove everything involved in this route */
        userRoutes
            .MapPatch("/", ChangeUsernameController.Invoke)
            .WithDescription("Change the username of a user")
            .RequireAuthorization()
            .WithName(nameof(ChangeUsernameController))
            .WithOpenApi();
    }

    private static void MapGameRoutes(WebApplication app)
    {
        RouteGroupBuilder gameRoutes = app.MapGroup("/api/games")
            .WithTags("Games")
            .WithDescription("All endpoints related to games")
            .WithOpenApi();

        gameRoutes
            .MapGet("/", GetGamesController.Invoke)
            .WithDescription("Get all games")
            .RequireAuthorization()
            .WithName(nameof(GetGamesController))
            .WithOpenApi();
        
        gameRoutes
            .MapPost("/{gameId}", StartGameController.Invoke)
            .WithDescription("Start a new game")
            .RequireAuthorization()
            .WithName(nameof(StartGameController))
            .WithOpenApi();

        gameRoutes
            .MapPatch("/{gameId}/artifacts/{artifactId}", UpdateGameStateController.Invoke)
            .WithDescription("Update a game state for a specific game for a specific user")
            .RequireAuthorization()
            .WithName(nameof(UpdateGameStateController))
            .WithOpenApi();
        
        gameRoutes
            .MapPost("/{gameId}/save", SaveGameController.Invoke)
            .WithDescription("Save a finished game")
            .RequireAuthorization()
            .WithName(nameof(SaveGameController))
            .WithOpenApi();

        gameRoutes
            .MapPost("/{gameId}/heartbeat", UpdateTtlController.Invoke)
            .WithDescription("Notify the server a game is still active")
            .RequireAuthorization()
            .WithName(nameof(UpdateTtlController))
            .WithOpenApi();
    }
}

using Adria.Infrastructure.WebApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        
        return app;
    }

    private static void MapUserRoutes(WebApplication app)
    {
        var userRoutes = app.MapGroup("/api/users")
            .WithTags("Users")
            .WithDescription("All endpoints related to Taskly todolists.")
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
    }
}

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record UserDto(
    Guid Id,
    string Username,
    string JwtToken
);
using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record CheckUsernameInUseInput(
    string Username
);

public sealed class CheckUsernameInUse : IUseCase<CheckUsernameInUseInput, Task<bool>>
{
    private readonly IUserExistsQuery _userNameQuery;
    private readonly ILogger<CheckUsernameInUse> _logger;

    public CheckUsernameInUse(
        IUserExistsQuery userNameQuery,
        ILogger<CheckUsernameInUse> logger
    )
    {
        _userNameQuery = userNameQuery;
        _logger = logger;
    }
    
    public async Task<bool> Execute(CheckUsernameInUseInput input)
    {
        _logger.LogInformation("Checking if username {username} exists", input.Username);

        return await _userNameQuery.Fetch(input.Username);
    }
}
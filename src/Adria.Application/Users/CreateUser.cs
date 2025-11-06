using Adria.Application.Contracts;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record CreateUserInput(
    string UserName,
    Avatar? Avatar
);

public sealed class CreateUser : IUseCase<CreateUserInput, Task<User>>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUser> _logger;

    public CreateUser(
        IUserRepository repository,
        ILogger<CreateUser> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<User> Execute(CreateUserInput input)
    {
        User user = new(input.UserName, input.Avatar);

        await _repository.Save(user);

        _logger.LogInformation("user {username} created", input.UserName);
        return user;
    }
}
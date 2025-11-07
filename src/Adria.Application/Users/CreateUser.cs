using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record CreateUserInput(
    string UserName
);
public sealed class CreateUser : IUseCase<CreateUserInput, Task<UserData>>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUser> _logger;
    private readonly IJwtProvider _jwtProvider;
    public CreateUser(
        IUserRepository repository,
        ILogger<CreateUser> logger,
        IJwtProvider jwtProvider
    )
    {
        _repository = repository;
        _logger = logger;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<UserData> Execute(CreateUserInput input)
    {
        User user = new(input.UserName);

        await _repository.Save(user);

        _logger.LogInformation("user {Username} created", input.UserName);

        string token = _jwtProvider.GenerateToken(user);
        return  new UserData(user, token); 
    }
}
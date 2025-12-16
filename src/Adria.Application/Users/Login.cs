using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed class Login : IUseCase<Guid, Task<UserData>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<Login> _logger;
    private readonly IJwtProvider _jwtProvider;
    public Login(
        IUserRepository repository,
        ILogger<Login> logger,
        IJwtProvider provider
    )
    {
        _userRepository = repository;
        _logger = logger;
        _jwtProvider = provider;
    }
    
    public async Task<UserData> Execute(Guid id)
    {
        _logger.LogInformation("Generating token for user {Id}", id);
        User? user = await _userRepository.ById(id);
        if (user is null)
        {
            _logger.LogInformation("User {Id} not found", id);
            throw new UserNotFoundException(id);
        }
        
        return new UserData(user, _jwtProvider.GenerateToken(user));
    }
}
using System.IdentityModel.Tokens.Jwt;
using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed class Login : IUseCase<Guid, Task<string>>
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
    
    public async Task<string> Execute(Guid id)
    {
        User user = await _userRepository.ById(id) ?? throw new ElementNotFoundException($"User with id {id} not found");
        return _jwtProvider.GenerateToken(user);
    }
}
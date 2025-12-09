using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;
public sealed class GetUser : IUseCase<Guid, Task<User>>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<GetUser> _logger;
    
    public GetUser(
        IUserRepository repository,
        ILogger<GetUser> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<User> Execute(Guid input)
    {
        _logger.LogInformation("Getting user with id {Id}", input);
        User? user = await _repository.ById(input);
        if (user is null) throw new UserNotFoundException(input);
        
        return user;
    }
}
using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public class GetUser : IUseCase<Guid, Task<User>>
{
    private readonly IUserRepository _repository;
    private readonly ILogger<ChangeUserName> _logger;
    
    public GetUser(
        IUserRepository repository,
        ILogger<ChangeUserName> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<User> Execute(Guid input)
    {
        User? user = await _repository.ById(input);
        if (user is null) throw new ElementNotFoundException($"user with id {input} not found");
        
        return user;
    }
}
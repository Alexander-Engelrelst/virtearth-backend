using Adria.Application.Authentication;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;
// TODO edit that all calls involving an id from the jwt token are called from a helper function (will be done in a separate issue)
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
        User? user = await _repository.ById(input);
        if (user is null) throw new ElementNotFoundException($"user with id {input} not found");
        
        return user;
    }
}
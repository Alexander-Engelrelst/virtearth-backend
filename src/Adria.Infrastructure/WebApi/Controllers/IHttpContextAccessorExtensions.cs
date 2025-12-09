using System.Security.Claims;
using Adria.Application.Contracts;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class HttpContextAccessorExtensions
{
    public static async Task<User> GetUser(this IHttpContextAccessor httpContextAccessor, IUseCase<Guid, Task<User>> getUser)
    {
        Claim? userClaim = httpContextAccessor.HttpContext?.User.FindFirst("guid");
        
        if (userClaim is null || !Guid.TryParse(userClaim.Value, out Guid id))
        {
           throw new NoUserIdInTokenException();
        }

        return await getUser.Execute(id);
    }
}
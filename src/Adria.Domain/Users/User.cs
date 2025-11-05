using System.Text.RegularExpressions;
using Adria.Domain.Shared.Exceptions;

namespace Adria.Domain.Users;

public sealed class User
{   
    
    private const int MAXIMUM_USERNAME_LENGTH = 40;
    private const int MINIMUM_USERNAME_LENGTH = 3;
    private static readonly Regex UsernameRegex = new Regex(
        @"^[a-zA-Z0-9._-]{3,40}$",
        RegexOptions.Compiled
    );
    public Guid Id { get; private init; }
    public string Username { get; private init; }
    public Avatar? Avatar { get; private init; }

    public User(
        string username,
        Avatar avatar
    )
    {
        EnsureValidUsername(username);
        
        Id = Guid.NewGuid();
        Username = username;
        Avatar = avatar;
    }

    public static void EnsureValidUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username) ||
            username.Length < MINIMUM_USERNAME_LENGTH ||
            username.Length > MAXIMUM_USERNAME_LENGTH ||
            !UsernameRegex.IsMatch(username))
        {
            throw new InvalidUsernameException(username);
        }
    }
}
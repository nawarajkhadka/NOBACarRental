using CarRental.Application.Auth.Models;

namespace CarRental.Infrastructure.Identity;

/// <summary>Adapts an <see cref="ApplicationUser"/> to the Application layer's <see cref="IAuthenticatedUser"/> abstraction.</summary>
public class AuthenticatedUser : IAuthenticatedUser
{
    public AuthenticatedUser(ApplicationUser user)
    {
        Id = user.Id;
        UserName = user.UserName ?? string.Empty;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }

    public int Id { get; }
    public string UserName { get; }
    public string FirstName { get; }
    public string LastName { get; }
}

namespace CarRental.Application.Auth.Models;

/// <summary>Abstraction over the Identity user so Application does not depend on ASP.NET Core Identity.</summary>
public interface IAuthenticatedUser
{
    int Id { get; }
    string UserName { get; }
    string FirstName { get; }
    string LastName { get; }
}

using CarRental.Application.Auth.Models;

namespace CarRental.Application.Auth;

public interface ITokenService
{
    string GenerateToken(IAuthenticatedUser user, IList<string> roles);
}

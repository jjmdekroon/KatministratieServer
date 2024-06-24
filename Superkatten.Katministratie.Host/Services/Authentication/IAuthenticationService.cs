using Superkatten.Katministratie.Host.Entities;

namespace Superkatten.Katministratie.Host.Services.Authentication;

public interface IAuthenticationService
{
    Task RegisterAsync(string username, string password, string name, string email);
    Task<User?> AuthenticateUserAsync(string username, string password);
}

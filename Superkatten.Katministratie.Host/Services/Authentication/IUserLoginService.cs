using Superkatten.Katministratie.Host.Entities;

namespace Superkatten.Katministratie.Host.Services.Authentication;

public interface IUserLoginService
{
    Task InitializeAsync();
    bool IsAuthenticated { get; }
    User? User { get; }
    Task SetUserAsync(User? user);
    Task ResetAsync();
}
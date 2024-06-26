using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.Services;
public class SuperSession : ISuperSession
{
    private User? _user;

    public void StartWithUser(User user)
    {
        _user = user;
    }

    public void Stop()
    {
        _user = null;
    }

    public User? User => _user;
}

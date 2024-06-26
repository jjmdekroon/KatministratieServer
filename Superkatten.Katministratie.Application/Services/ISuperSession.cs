using Superkatten.Katministratie.Domain.Entities;

namespace Superkatten.Katministratie.Application.Services;

public interface ISuperSession
{
    void StartWithUser(User user);
    void Stop();

    User? User { get; }
}
using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Services.Interfaces;

public interface ICatchOriginService
{
    Task<IReadOnlyCollection<CatchOrigin>> GetCatchOriginsAsync();
}

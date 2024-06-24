using Superkatten.Katministratie.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Interfaces;

public interface ICatchOriginRepository
{
    Task<CatchOrigin> CreateCatchOriginAsync(CatchOriginType type, string name);
    Task<CatchOrigin?> GetCatchOriginAsync(CatchOriginType type, string name);
    Task<IReadOnlyCollection<CatchOrigin>> GetCatchOriginsAsync();
}

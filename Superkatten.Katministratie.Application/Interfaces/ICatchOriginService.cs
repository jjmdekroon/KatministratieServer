using Superkatten.Katministratie.Contract.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Application.Interfaces;

public interface ICatchOriginService
{
    Task<IReadOnlyCollection<CatchOrigin>> GetCatchOriginsAsync();
}

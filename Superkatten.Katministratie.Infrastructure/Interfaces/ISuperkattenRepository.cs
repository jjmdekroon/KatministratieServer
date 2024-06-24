using Superkatten.Katministratie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Interfaces
{
    public interface ISuperkattenRepository
    {
        Task CreateSuperkatAsync(Superkat superkat);
        Task<Superkat> GetSuperkatAsync(Guid id);
        Task DeleteSuperkatAsync(Guid guid);
        Task UpdateSuperkatAsync(Superkat superkat);
        Task<IReadOnlyCollection<Superkat>> GetSuperkattenAsync();
        Task<int> GetMaxSuperkatNumberForYear(int year);
    }
}

using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Domain.Entities.Locations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superkatten.Katministratie.Infrastructure.Interfaces
{
    public interface ILocationRepository
    {
        Task CreateLocationAsync(BaseLocation location);
        Task<BaseLocation> GetLocationAsync(Guid locationId);
        Task UpdateLocationAsync(BaseLocation location);
        Task DeleteLocationAsync(Guid id);        
        Task<IReadOnlyCollection<BaseLocation>> GetLocationsAsync();

        Task<LocationNaw> GetLocationNawAsync(string name);
    }
}

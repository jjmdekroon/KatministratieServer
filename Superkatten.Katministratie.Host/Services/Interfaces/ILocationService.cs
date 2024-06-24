using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities.Locations;

namespace Superkatten.Katministratie.Host.Services;

public interface ILocationService
{
    Task<List<Location>> GetLocationsAsync();
    Task<List<Location>> GetLocationsByTypeAsync(LocationType locationType);
    Task<Location?> GetLocationAsync(Guid locationGuid);
    Task<Location?> CreateLocationAsync(LocationNawParameters locationNawParameters);
    Task<Location?> UpdateLocationAsync(Guid id, LocationNawParameters updateNawLocationParameters);
    Task DeleteLocationAsync(Guid id);
    Task<Location?> GetAdopterByGuidAsync(Guid guid);
}


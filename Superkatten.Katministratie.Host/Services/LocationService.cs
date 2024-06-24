using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Host.Services.Http;

namespace Superkatten.Katministratie.Host.Services;

public class LocationService : ILocationService
{
    private readonly IHttpService _httpService;

    public LocationService(IHttpService httpService)
    {
        _httpService = httpService;
    }


    public async Task<Location?> CreateLocationAsync(LocationNawParameters locationNawParameters)
    {
        var uri = "api/Location";
        var response = await _httpService.Put<Location>(uri, locationNawParameters);
        return response;
    }

    public async Task<Location?> UpdateLocationAsync(Guid id, LocationNawParameters locationNawParameters)
    {
        var uri = $"api/Location?Id={id}";
        var response = await _httpService.Post<Location>(uri, locationNawParameters);
        return response;
    }

    public async Task DeleteLocationAsync(Guid id)
    {
        var uri = $"api/Location?Id={id}";
        await _httpService.Delete(uri);
    }

    public async Task<Location?> GetLocationAsync(Guid id)
    {
        var locations = await GetLocationsAsync();

        return locations
            .Where(s => s.Id == id)
            .First();
    }
    
    public async Task<List<Location>> GetLocationsAsync()
    {
        var uri = "api/Location";
        var locations = await _httpService.Get<List<Location>>(uri); 

        return locations is null
            ? new()
            : locations.ToList();
    }

    public async Task<List<Location>> GetLocationsByTypeAsync(LocationType locationType)
    {
        var uri = "api/Location";
        var locations = await _httpService.Get<List<Location>>(uri);

        return locations is null
            ? new()
            : locations
                .Where(o => o.LocationType == locationType)
                .ToList();
    }

    public async Task<Location?> GetAdopterByGuidAsync(Guid guid)
    {
        var uri = $"api/Location/adopter?Id={guid}";
        var adopter= await _httpService.Get<Location>(uri);

        return adopter;
    }
}

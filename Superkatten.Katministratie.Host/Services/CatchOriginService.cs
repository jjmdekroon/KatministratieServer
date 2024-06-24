using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Services.Http;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Services;

public class CatchOriginService : ICatchOriginService
{
    private readonly IHttpService _httpService;

    public CatchOriginService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<IReadOnlyCollection<CatchOrigin>> GetCatchOriginsAsync()
    {
        var uri = "api/CatchOrigin";

        try
        {
            var allLocations = await _httpService.Get<List<CatchOrigin>>(uri);

            return allLocations is null
                ? new List<CatchOrigin>()
                : allLocations
                    .DistinctBy(o => o.Name)
                    .ToList();
        }
        catch
        {
            return new List<CatchOrigin>();
        }
    }
}

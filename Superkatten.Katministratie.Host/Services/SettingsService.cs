using Microsoft.AspNetCore.Mvc;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Services.Http;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Services;

public class SettingsService : ISettingsService
{
    private readonly IHttpService _httpService;

    public SettingsService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<IReadOnlyCollection<int>> GetCageNumbersForCatAreaAsync(CatArea catArea)
    {
        var uri = $"api/Settings/CageNumbersForCatArea";
        var cageNumbers = await _httpService.Put<List<int>>(uri, catArea);

        return cageNumbers is null
            ? Array.Empty<int>()
            : cageNumbers;
    }
}

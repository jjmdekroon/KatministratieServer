
using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;

namespace Superkatten.Katministratie.Host.Pages.GastgezinPages;

public partial class OverviewGastgezinnen
{
    [Inject]
    private Navigation _navigation { get; set; } = null!;

    [Inject]
    private ILocationService _gastgezinnenService { get; set; } = null!;



    private List<Location>? _gastgezinnen = null;

    protected override async Task OnInitializedAsync()
    {
        await UpdateListAsync();
    }

    private async Task UpdateListAsync()
    {
        var locations = await _gastgezinnenService.GetLocationsByTypeAsync(LocationType.HostFamily);

        _gastgezinnen = locations is null
            ? new List<Location>()
            : locations.ToList();
    }

    private void OnBackHome()
    {
        _navigation.NavigateTo("/");
    }

    public void OnCreateGastgezin()
    {
        _navigation.NavigateTo("CreateGastgezin");
    }

    public void OnGastgezinDeleted(Location gastgezin)
    {
        _gastgezinnen.Remove(gastgezin);
    }
}

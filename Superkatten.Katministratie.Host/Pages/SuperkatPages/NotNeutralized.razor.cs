using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using System.ComponentModel;

namespace Superkatten.Katministratie.Host.Pages.SuperkatPages;

public partial class NotNeutralized
{
    [Inject] private Navigation _navigation { get; set; } = null!;

    [Inject] private ISuperkattenListService _superkattenListService { get; set; } = null!;

    private List<Superkat> Superkatten { get; set; } = new();
    private static List<string> _neutralizeFilterOptionNames = null!;

    private readonly static List<IsNeutralizedAtLocationView> _neutralizeFilterOptions = Enum
    .GetValues(typeof(IsNeutralizedAtLocationView))
    .Cast<IsNeutralizedAtLocationView>()
    .ToList();


    protected override Task OnInitializedAsync()
    {
        _neutralizeFilterOptionNames = _neutralizeFilterOptions
            .Select(x => x.ToString())
            .ToList();

        return Task.CompletedTask;
    }

    private async Task UpdateListAsync(IsNeutralizedAtLocationView filter)
    {
        var unsortedSuperkatten = await _superkattenListService.GetAllNotNeutralizedSuperkattenAsync();

        var superkatten = unsortedSuperkatten.OrderBy(o => o.UniqueNumber).ToList();

        Superkatten = filter switch
        {
            IsNeutralizedAtLocationView.All => superkatten,
            IsNeutralizedAtLocationView.Refuge => superkatten.Where(o => o.Location.LocationType is LocationType.Refuge).ToList(),
            IsNeutralizedAtLocationView.HostFamily => superkatten.Where(o => o.Location.LocationType is LocationType.HostFamily).ToList(),
            _ => throw new InvalidEnumArgumentException(nameof(filter), (int)filter, typeof(IsNeutralizedAtLocationView))
        };
    }

    private async Task OnSelectFilter(IsNeutralizedAtLocationView selectedFilter)
    {
        await UpdateListAsync(selectedFilter);
    }

    private void OnBackHome()
    {
        _navigation.NavigateTo("/");
    }
}

using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class SuperkatSelectorComponent : ComponentBase
{
    [Parameter] public List<Superkat> AvailableSuperkatten { get; set; } = null!;
    [Parameter] public List<Superkat> SelectedSuperkatten { get; set; } = null!;
    [Parameter] public EventCallback<Superkat> AddSuperkat { get; set; }
    [Parameter] public EventCallback<Superkat> RemoveSuperkat { get; set; }
    [Parameter] public string AvailableCollectionHeader { get; set; } = "Keuze uit:";
    [Parameter] public string SelectedCollectionHeader { get; set; } = "Geselecteerd:";
    [Parameter] public bool EnableOneWay { get; set; } = false;

    private readonly List<Superkat> _availableSuperkatten = new();
    private readonly List<Superkat> _selectedSuperkatten = new();

    protected override Task OnInitializedAsync()
    {
        if (AvailableSuperkatten is not null)
        {
            _availableSuperkatten.AddRange(AvailableSuperkatten);
        }

        if (SelectedSuperkatten is not null)
        {
            _selectedSuperkatten.AddRange(SelectedSuperkatten);
        }

        return Task.CompletedTask;
    }

    private async Task AddSuperkatToSelectionAsync(Superkat superkat)
    {
        _availableSuperkatten?.Remove(superkat);
        _selectedSuperkatten?.Add(superkat);

        await AddSuperkat.InvokeAsync(superkat);
    }

    private async Task RemoveSuperkatFromSelectionAsync(Superkat superkat)
    {
        _availableSuperkatten?.Add(superkat);
        _selectedSuperkatten?.Remove(superkat);

        await RemoveSuperkat.InvokeAsync(superkat);
    }
}

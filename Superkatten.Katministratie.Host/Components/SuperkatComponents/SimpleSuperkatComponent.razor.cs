
using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class SimpleSuperkatComponent : ComponentBase
{
    [Inject] private ISuperkattenListService _superkattenService { get; set; } = null!;

    [Inject] private Navigation Navigation { get; set; } = null!;


    [Parameter]
    public Superkat? Superkat
    {
        set
        {
            if (value is null)
            {
                return;
            }

            SuperkatView = new SuperkatView(value);
        }
    }

    private SuperkatView SuperkatView { get; set; } = null!;

    private async Task ReloadSuperkatDataAsync()
    {
        if (SuperkatView is null)
        {
            return;
        }

        var superkat = await _superkattenService.GetSuperkatAsync(SuperkatView.Superkat.Id);
        SuperkatView = new SuperkatView(superkat);
    }

    private void OnEditSuperkat()
    {
        Navigation.NavigateTo($"EditSuperkat/{SuperkatView.Superkat.Id}");
    }
}

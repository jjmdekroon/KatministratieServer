
using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class SuperkatComponent : ComponentBase
{
    [Inject] private ISuperkattenListService _superkattenService { get; set; } = null!;
    [Inject] public Navigation Navigation { get; set; } = null!;


    [Parameter]
    public Superkat? Superkat
    {
        set
        {
            if (value is null)
            {
                return;
            }

            _superkatView = new SuperkatView(value);
        }
    }

    private SuperkatView? _superkatView;

    private async Task ReloadSuperkatData()
    {
        if (_superkatView is null)
        {
            return;
        }

        var superkat = await _superkattenService.GetSuperkatAsync(_superkatView.Superkat.Id);
        _superkatView = new SuperkatView(superkat);
    }

    private static string GetSuperkatImage(byte[] imageData)
    {
        return $"data:image/png;base64, {Convert.ToBase64String(imageData)}";
    }

    private void OnClickCreatePhoto(Superkat superkat)
    {
        Navigation.NavigateTo($"CreateSuperkatPhoto/{superkat.Id}");
    }
}


using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Helpers;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents.Action;

public partial class SuperkatActions : ComponentBase
{
    [Inject]
    public Navigation Navigation { get; set; } = null!;

    [Parameter]
    public SuperkatView SuperkatView { get; set; } = null!;

    [Parameter]
    public EventCallback OnActionExecuted { get; set; }


    private async Task ToggleReserve()
    {
        await _superkatActionService.ToggleReserveSuperkatAsync(SuperkatView.Superkat.Id);
        await OnActionExecuted.InvokeAsync();
    }

    private async Task ToggleRetour()
    {
        await _superkatActionService.ToggleRetourSuperkatAsync(SuperkatView.Superkat.Id);
        await OnActionExecuted.InvokeAsync();
    }

    private void AddMedicalProcedure()
    {
        Navigation.NavigateTo($"AddMedicalProcedure/{SuperkatView.Superkat.Id}");
    }

    private void ShowSuperkatState()
    {
        Navigation.NavigateTo($"SuperkatStatus/{SuperkatView.Superkat.Id}");
    }
}

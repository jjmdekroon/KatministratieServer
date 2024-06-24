using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Helpers;

namespace Superkatten.Katministratie.Host.Pages.Adoption;

public partial class AdoptersOverview
{
    [Inject] public Navigation Navigation { get; set; } = null!;

    private IReadOnlyCollection<Adoptant> _adopters = null!;

    protected override Task OnInitializedAsync()
    {
        // Lees de adoptanten waarvan de adoptie nog niet is afgerond.
        _adopters = new List<Adoptant>();

        return Task.CompletedTask;
    }

    private void OnCancel()
    {
        Navigation.NavigateBack();
    }
}
using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class SuperkattenListComponent
{
    [Parameter]
    public IReadOnlyCollection<Superkat> SuperkatItems { get; set; } = null!;
}
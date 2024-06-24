using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Host.Entities;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class SuperkatDetailsComponent : ComponentBase
{ 
    [Parameter]
    public SuperkatView? SuperkatView { get; set; }
}

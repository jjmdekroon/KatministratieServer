using Microsoft.AspNetCore.Components;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents;

public partial class SuperkatGenderComponent : ComponentBase
{
    [Parameter]
    public string GenderIcon { get; set; } = string.Empty;
}

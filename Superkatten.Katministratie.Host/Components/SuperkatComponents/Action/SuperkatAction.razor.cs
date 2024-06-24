
using Microsoft.AspNetCore.Components;

namespace Superkatten.Katministratie.Host.Components.SuperkatComponents.Action;

public partial class SuperkatAction : ComponentBase
{
    [Parameter]
    public string ActionIcon { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnClickCallback { get; set; }

    public async void ExecuteAction()
    {
        await OnClickCallback.InvokeAsync();
    }
}

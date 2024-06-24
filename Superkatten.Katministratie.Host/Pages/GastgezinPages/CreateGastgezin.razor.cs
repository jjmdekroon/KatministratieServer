using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Services;

namespace Superkatten.Katministratie.Host.Pages.GastgezinPages;

public partial class CreateGastgezin
{

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public ILocationService GastgezinService { get; set; } = null!;


    public HostFamilyNawData GastgezinData { get; set; } = new();
                    
    private bool ValidName => string.IsNullOrWhiteSpace(GastgezinData?.Name);

    public async Task OnOk()
    {
        if(NavigationManager is null)
        {
            return;
        }

        await StoreGastgezin();
        NavigationManager.NavigateTo("OverviewGastgezinnen");
    }

    public void OnCancel()
    {
        if (NavigationManager is null)
        {
            return;
        }

        NavigationManager.NavigateTo("OverviewGastgezinnen");
    }

    private async Task StoreGastgezin()
    {
        var createGastgezin = new LocationNawParameters
        {
            Name = GastgezinData.Name,
            Address = GastgezinData.Address,
            Postcode = GastgezinData.Postcode,
            City = GastgezinData.City,
            Phone = GastgezinData.Phone,
            Email = GastgezinData.Email
        };

        var gastgezin = await GastgezinService.CreateLocationAsync(createGastgezin);
        if (gastgezin is null)
        {
//TODO            await Message.Error($"Fout bij het opslaan van een nieuw gastgezin.", 1);
            return;
        }

//TODO        await Message.Success($"Gastgezin {gastgezin.Name} is opgeslagen.", 1);        
    }
}

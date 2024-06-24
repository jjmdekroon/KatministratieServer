using Blazorise;
using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Authentication;

namespace Superkatten.Katministratie.Host.Pages.GastgezinPages;

public partial class SuperkatAdoption
{
    [Inject] public Navigation Navigation { get; set; } = null!;
    [Inject] public ISuperkattenListService SuperkattenService { get; set; } = null!;
    [Inject] public ISuperkatActionService SuperkattenActionService { get; set; } = null!;
    [Inject] public ILocationService GastegezinService { get; set; } = null!;

    [Parameter] public Guid GastgezinId { get; set; }

    private List<Superkat> _assignedSuperkatten = null!;
    private List<Superkat> _selectedSuperkatten = new();
    private string _emailAddress = string.Empty;
    private string _name = string.Empty; 
    private Location? _gastgezin;
    private bool _disableContinueButton => 
        string.IsNullOrWhiteSpace(_emailAddress) ||
        string.IsNullOrWhiteSpace(_name) ||
        !_selectedSuperkatten.Any(); 

    protected override async Task OnInitializedAsync()
    {
        _gastgezin = await GastegezinService.GetLocationAsync(GastgezinId);

        var superkatten = await SuperkattenService.GetAllSuperkattenAsync();
        var assignedSuperkatten = superkatten
            .Where(o => o.Location.Id == _gastgezin?.Id)
            .OrderBy(s => s.Number)
            .ToList();
        
        if (assignedSuperkatten is null)
        {
            throw new Exception("No assigned superkatten available");
        }

        _selectedSuperkatten = assignedSuperkatten
            .Where(s => s.State != SuperkatState.New)
            .ToList();

        _assignedSuperkatten = assignedSuperkatten
            .Where(s => s.State == SuperkatState.New)
            .ToList();
    }
    private void ValidateEmail(ValidatorEventArgs e)
    {
        var email = Convert.ToString(e.Value);

        e.Status = string.IsNullOrEmpty(email) ? ValidationStatus.None :
            email.Contains("@") ? ValidationStatus.Success : ValidationStatus.Error;
    }

    private void OnSendAdoptionPapers()
    {
        var adoptees = _selectedSuperkatten
            .Select(s => s.Id)
            .ToList();
        SuperkattenActionService.AdoptSuperkatten(GastgezinId, adoptees, _name, _emailAddress);
        Navigation.NavigateBack();
    }

    private void OnBack()
    {
        Navigation.NavigateBack();
    }

    private void AddSuperkatToSelectionAsync(Superkat superkat)
    {
        _assignedSuperkatten.Remove(superkat);
        _selectedSuperkatten.Add(superkat);
    }

    private void RemoveSuperkatFromSelection(Superkat superkat)
    {
        _selectedSuperkatten.Remove(superkat);
        _assignedSuperkatten.Add(superkat);
    }
}
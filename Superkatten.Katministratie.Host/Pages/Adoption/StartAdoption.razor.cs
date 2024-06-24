using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Pages.Users;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Authentication;

namespace Superkatten.Katministratie.Host.Pages.Adoption;

public partial class StartAdoption
{
    [Inject] private ILocationService LocationService { get; set; } = null!;
    [Inject] private ISuperkattenListService SuperkattenService { get; set; } = null!;
    [Inject] private Navigation Navigation { get; set; } = null!;
    [Inject] public IUserLoginService UserLoginService { get; set; } = null!;
    [Inject] public IAuthenticationService AuthenticationService { get; set; } = null!;

    [Parameter] public Guid AdopterGuid { get; set; }

    private IReadOnlyCollection<Superkat> _superkatten = new List<Superkat>();
    private TextEdit? _textEditLoginName;

    private LoginModel _loginModel = new();
    private Modal? _authenticationDialog = null!;
    private bool _isLoggingIn = false;

    private string _adopterName = string.Empty;
    private string _adopterAddress = string.Empty;
    private string _adopterPostcode = string.Empty;
    private string _adopterCity = string.Empty;
    private string _adopterPhone = string.Empty;
    private string _adopterEmail = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        if (_authenticationDialog is null)
        {
            return;
        }

        _loginModel = new();
        await _authenticationDialog.Show();
    }

    private async Task GetAdopterDataAsync()
    {
        var adopter = await LocationService.GetAdopterByGuidAsync(AdopterGuid);
        if (adopter is null)
        {
            return;
        }

        var superkatten = await SuperkattenService.GetAllSuperkattenAsync();
        _superkatten = superkatten
            .Where(o => o.Location.Id == AdopterGuid)
            .ToList();

        _adopterName = adopter.Naw.Name;
        _adopterAddress = adopter.Naw.Address ?? string.Empty;
        _adopterPostcode = adopter.Naw.Postcode ?? string.Empty;
        _adopterCity = adopter.Naw.City ?? string.Empty;
        _adopterPhone = adopter.Naw.Phone ?? string.Empty;
        _adopterEmail = adopter.Naw.Email ?? string.Empty;
    }
    private async Task OnKeyPress(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Key != "Enter" || _loginModel is not null && !_loginModel.AllFilledIn)
        {
            return;
        }

        await SaveAndHideModal();
    }
    private Task HideModal()
    {
        if (_authenticationDialog is null)
        {
            return Task.CompletedTask;
        }

        return _authenticationDialog.Hide();
    }

    private async Task SaveAndHideModal()
    {
        if (_authenticationDialog is null)
        {
            return;
        }

        _isLoggingIn = true;

        if (_loginModel is null)
        {
            return;
        }

        var user = await AuthenticationService.AuthenticateUserAsync(_loginModel.Username, _loginModel.Password);
        await UserLoginService.SetUserAsync(user);

        if (user is null)
        {
            return;
        }

        await GetAdopterDataAsync();

        await _authenticationDialog.Hide();

        _isLoggingIn = false;
    }

    private async Task OnOk()
    {
        var locationNaw = new LocationNawParameters
        { 
            Name = _adopterName,
            Address = _adopterAddress,
            Postcode = _adopterPostcode,
            City = _adopterCity,
            Phone = _adopterPhone,
            Email= _adopterEmail
        };

        _ = await LocationService.UpdateLocationAsync(AdopterGuid, locationNaw);

        Navigation.NavigateTo("www.superkatten.nl");
    }

    private void OnCancel()
    {
        Navigation.NavigateTo("www.superkatten.nl");
    }
}

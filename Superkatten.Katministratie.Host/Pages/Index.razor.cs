using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Superkatten.Katministratie.Contract.ApiInterface.Reporting;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Pages.Users;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Authentication;
using Superkatten.Katministratie.Host.Services.Interfaces;
using Superkatten.Katministratie.Contract.Language;

namespace Superkatten.Katministratie.Host.Pages;

public partial class Index
{
    [Inject] IStringLocalizer<KatministratieApp> Localizer { get; set; } = null!;
    [Inject] IPageProgressService PageProgressService { get; set; } = null!;
    [Inject] public Navigation Navigation { get; set; } = null!;
    [Inject] public ISuperkattenListService SuperkattenService { get; set; } = null!;
    [Inject] public IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] public IUserLoginService UserLoginService { get; set; } = null!;
    [Inject] public IReportingService ReportingService { get; set; } = null!;


    private LoginModel _loginModel = new();
    private Modal? _authenticationDialog = null!;
    private bool _isLoggingIn = false;
    private int _superkattenInRefugeCount;
    private int _superkattenNotNeutralizedCount;

    private string KatministratieTitel => Localizer["KatministrationTitle"].Value;
    private string LoginBodyText => Localizer["LoginBodyText"].Value;

    protected override async Task OnInitializedAsync()
    {
        await UserLoginService.InitializeAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (UserLoginService.IsAuthenticated)
        {
            await UpdateLabelMarkersAsync();
            StateHasChanged();
        }
    }

    private void OnRegister()
    {
        Navigation.NavigateTo("Register");
    }

    private TextEdit? _textEditLoginName;

    private void OnCreateSuperkatPhoto()
    {
        Navigation.NavigateTo("CreateSuperkatPhoto");
    }
    private void OnMoveSuperkat()
    {
        Navigation.NavigateTo("MoveSuperkat");
    }

    private void OnCreateSuperkat()
    {
        Navigation.NavigateTo("CreateSuperkat");
    }

    private void OnShowOverviewSuperkatten()
    {
        Navigation.NavigateTo("OverviewSuperkatten");
    }

    private void OnShowNotNeutralized()
    {
        Navigation.NavigateTo("NotNeutralized");
    }

    private void OnShowOverviewGastgezinnen()
    {
        Navigation.NavigateTo("OverviewGastgezinnen");
    }

    private void OnShowMedicalProcedures()
    {
        Navigation.NavigateTo("OverviewMedicalProcedures");
    }

    private void OnShowSuperkatStatus()
    {
        Navigation.NavigateTo("SuperkatStatus");
    }
    private void OnShowReportingPage()
    {
        Navigation.NavigateTo("ReportsPage");
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

        await PageProgressService.Go(null, options => { options.Color = Color.Info;  });

        if (_loginModel is not null)
        {
            var user = await AuthenticationService.AuthenticateUserAsync(_loginModel.Username, _loginModel.Password);
            await UserLoginService.SetUserAsync(user);
        }

        await _authenticationDialog.Hide();
        await PageProgressService.Go(-1);

        _isLoggingIn = false;
    }

    private async Task UpdateLabelMarkersAsync()
    {
        var superkattenInRefuge = await SuperkattenService.GetAllSuperkattenAsync();
        var superkattenNotNeutralized = await SuperkattenService.GetAllNotNeutralizedSuperkattenAsync();

        _superkattenInRefugeCount = superkattenInRefuge
            .Count(s => s.Location.LocationType is LocationType.Refuge);

        _superkattenNotNeutralizedCount = superkattenNotNeutralized.Count;
    }

    private async Task OnKeyPress(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Key != "Enter" || _loginModel is not null && !_loginModel.AllFilledIn)
        {
            return;
        }

        await SaveAndHideModal();
    }


    private async Task OnCreateWaardigDierInventoryReport()
    {
        var email = UserLoginService?.User?.Email;
        if (string.IsNullOrEmpty(email))
        {
            //           _notificationString = "Email van ingelogde gebruiker is niet ingevuld. De email kan niet worden verstuurd.";
            //           await _notification.Show();
        }

        var requestParameters = new RequestCatchOriginEmailParameters
        {
            Email = email ?? string.Empty,
            From = DateTime.UtcNow.AddMonths(-3),
            To = DateTime.UtcNow
        };

        await ReportingService.EmailInventoryDetailsReportAsync(requestParameters);
    }

    /*private async Task OnNotNeutralizedInRefugeReport()
    {
        var email = UserLoginService?.User?.Email ?? string.Empty;

        await ReportingService.EmailNotNeutralizedInRefugeReportAsync(email);
    }

    private async Task OnNotNeutralizedAdopteesReport()
    {

        var email = UserLoginService?.User?.Email ?? string.Empty;

        await ReportingService.EmailNotNeutralizedAdopteesReportAsync(email);
    }*/

    public async Task OnLoginLogout()
    {
        if (_authenticationDialog is null)
        {
            return;
        }

        if (UserLoginService.User is null)
        {
            _loginModel = new();
            await _authenticationDialog.Show();
            return;
        }

        Navigation.NavigateTo("/");
        await UserLoginService.ResetAsync();
    }
}
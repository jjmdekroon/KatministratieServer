using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Superkatten.Katministratie.Contract.ApiInterface.Reporting;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Contract.Entities.Locations;
using Superkatten.Katministratie.Contract.Language;
using Superkatten.Katministratie.Host.Components;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Authentication;
using Superkatten.Katministratie.Host.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Superkatten.Katministratie.Host.Pages.Reports;

partial class CageCard
{
    [Inject] private IStringLocalizer<KatministratieApp> Localizer { get; set; } = null!;
    [Inject] private ISettingsService SettingsService { get; set; } = null!;
    [Inject] private ISuperkattenListService SuperkattenService { get; set; } = null!;
    [Inject] public IAuthenticationService AuthenticationService { get; init; } = null!;
    [Inject] public IUserLoginService UserLoginService { get; init; } = null!;
    [Inject] public IReportingService ReportingService { get; init; } = null!;
    [Inject] public Navigation Navigation { get; init; } = null!;

    private bool _isSending = false;
    private IReadOnlyCollection<Superkat> Superkatten { get; set; } = Array.Empty<Superkat>();

    private static List<CatArea> _catAreas = null!;
    private static List<string> _catAreaNames = null!;

    private static IReadOnlyCollection<int> _cageNumbers = null!;
    private static IReadOnlyCollection<string> _cageNumberNames = null!;

    private CatArea _selectedCatArea;
    private int _selectedCageNumber;

    private string SuperkattenMessage
    {
        get
        {
            if (_selectedCageNumber == 0)
            {
                return string.Empty;
            }

            if (Superkatten.Count == 0)
            {
                return "Er zitten geen superkatten in deze kooi";
            }

            if (Superkatten.Count > 1)
            {
                return $"Er zitten {Superkatten.Count} katten in deze kooi";
            }

            return $"Er zit 1 superkat in deze kooi met nunmmer {Superkatten.First().UniqueNumber}";
        }
    }

    [MemberNotNull(nameof(_catAreas), nameof(_catAreaNames))]
    protected override Task OnInitializedAsync()
    {
        _catAreas = Enum.GetValues(typeof(CatArea)).Cast<CatArea>().ToList();
        _catAreaNames = _catAreas.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();

        return Task.CompletedTask;
    }

    public async Task UpdateCatAreaDataAsync(CatArea catArea)
    {
        _selectedCatArea = catArea;
        _cageNumbers = Array.Empty<int>();

        var cageNumbers = await SettingsService.GetCageNumbersForCatAreaAsync(catArea);
        _cageNumbers = cageNumbers is null
            ? Array.Empty<int>()
            : cageNumbers;
        
        var cageNumberNames = _cageNumbers.Select(x => x.ToString()).ToList();
        _cageNumberNames = cageNumberNames;

        var firstCage = _cageNumbers.FirstOrDefault();
        await UpdateSuperkattenListAsync(firstCage);
    }

    public async Task UpdateSuperkattenListAsync(int selectedCageNumber)
    {
        _selectedCageNumber = selectedCageNumber;
        var activeSuperkattenList = await SuperkattenService.GetAllNotAssignedSuperkattenAsync();

        var superkatten = new List<Superkat>();
        foreach (var superkat in activeSuperkattenList)
        {
            try
            {
                var location = superkat.Location as Refuge;
                if (location is null)
                {
                    // What to do here ?

                }
                else if (location.CatArea == _selectedCatArea && location.CageNumber == selectedCageNumber)
                {
                    superkatten.Add(superkat);
                }
            }
            catch(Exception ex) 
            { 
            }
        }
        
        Superkatten = superkatten
            .OrderBy(s => s.UniqueNumber)
            .ToList();
    }

    private async Task OnOk()
    {
        _isSending = true;

        try
        {
            var email = UserLoginService?.User?.Email;
            if (email is null || string.IsNullOrEmpty(email))
            {
                return;
            }

            var parameters = new RequestCageCardEmailParameters
            {
                Email = email,
                CageNumber = _selectedCageNumber,
                CatArea = _selectedCatArea
            };

            await ReportingService.EmailCageCardAsync(parameters);
        }
        finally 
        {
            _isSending = false;
            Navigation.NavigateBack();
        }
    }

    public void OnCancel()
    {
        Navigation.NavigateBack();
    }
}


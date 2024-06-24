using Blazorise;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Contract.Language;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Pages.SuperkatPages;

public class CatColor
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public partial class CreateSuperkat
{
    public const int MAX_HOKNUMBER_ALLOWED = 50;
    private const int NOTIFICATION_SHOW_TIME = 2500;

    [Inject] IStringLocalizer<KatministratieApp> Localizer { get; set; } = null!;
    [Inject] public Navigation Navigation { get; set; } = null!;
    [Inject] public ISuperkattenListService SuperkattenService { get; set; } = null!;
    [Inject] public ICatchOriginService CatchOriginService { get; set; } = null!;
    [Inject] private ISettingsService SettingsService { get; set; } = null!;

    private static readonly CreateSuperkatSelections _selections = new();

    public DateTime? CatchDate = DateTime.UtcNow;
    private Guid _catchOriginNameId = Guid.NewGuid();
    public string CatchOriginName = string.Empty;
    public int CageNumber = 1;
    public bool Retour = false;
    public bool WetFoodAllowed = true;
    public string CatColor = string.Empty;
    public bool StrongHoldGiven = false;
    public int EstimatedWeeksOld = 0;
    private SnackbarStack? _snackbarStack;
    public IEnumerable<CatchOrigin> CatchOrigins = new List<CatchOrigin>();
    public IEnumerable<CatColor> CatColors = new List<CatColor>();

    private readonly static List<CatchOriginType> _catchOriginTypes = Enum.GetValues(typeof(CatchOriginType)).Cast<CatchOriginType>().ToList();
    private readonly static List<CatBehaviour> _catBehaviourTypes = Enum.GetValues(typeof(CatBehaviour)).Cast<CatBehaviour>().ToList();
    private readonly static List<AgeCategory> _ageCategoryTypes = Enum.GetValues(typeof(AgeCategory)).Cast<AgeCategory>().ToList();
    private readonly static List<CatArea> _catAreaTypes = Enum.GetValues(typeof(CatArea)).Cast<CatArea>().ToList();
    private readonly static List<Gender> _genderTypes = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
    private readonly static List<FoodType> _foodTypes = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().ToList();
    private readonly static List<LitterGranuleType> _litterGranuleTypes = Enum.GetValues(typeof(LitterGranuleType)).Cast<LitterGranuleType>().ToList();

    private static List<string> _catchOriginTypeNames = null!;
    private static List<string> _catBehaviourTypeNames = null!;
    private static List<string> _ageCategoryTypeNames = null!;
    private static List<string> _catAreaTypeNames = null!;
    private static List<string> _genderTypeNames = null!;
    private static List<string> _foodTypeNames = null!;
    private static List<string> _litterGranuleTypeNames = null!;

    private static IReadOnlyCollection<int> _cageNumbers = Array.Empty<int>();
    private static IReadOnlyCollection<string> _cageNumberNames = Array.Empty<string>();

    private string _selectedSearchValue = string.Empty;

    private bool _isInitialized = false;

    public async Task OnSelectCatArea(CatArea catArea)
    {
        _selections.Store(catArea);

        _cageNumbers = Array.Empty<int>();
        StateHasChanged();

        var cageNumbers = await SettingsService.GetCageNumbersForCatAreaAsync(catArea);
        _cageNumbers = cageNumbers is null
            ? Array.Empty<int>()
            : cageNumbers;
        _cageNumberNames = _cageNumbers.Select(x => x.ToString()).ToList();

        var selectedCageNumber = _cageNumbers.FirstOrDefault();
        _selections.Store(selectedCageNumber);

        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        _litterGranuleTypeNames = _litterGranuleTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();
        _catchOriginTypeNames = _catchOriginTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();
        _catBehaviourTypeNames = _catBehaviourTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();
        _ageCategoryTypeNames = _ageCategoryTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();
        _catAreaTypeNames = _catAreaTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();
        _genderTypeNames = _genderTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();
        _foodTypeNames = _foodTypes.Select(x => Localizer[x.GetType().Name + x.ToString()].Value).ToList();

        CatchOrigins = await CatchOriginService.GetCatchOriginsAsync();

        var allSuperkatten = await SuperkattenService.GetAllSuperkattenAsync();
        var colors = allSuperkatten
            .Select(s => s.Color)
            .Distinct();

        var catColors = colors
            .Select((value, index) => new CatColor
                {
                    Id = $"{index}",
                    Name = value
                })
            .ToList();

        CatColors = catColors;

        _isInitialized = true;
    }

    public async Task OnAddSuperkat()
    {
        await StoreSuperkatAsync();
    }

    public void OnStopAndReturn()
    {
        Navigation?.NavigateBack();
    }

    private async Task<bool> StoreSuperkatAsync()
    {
        if (SuperkattenService is null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(CatchOriginName))
        {
            await _snackbarStack!.PushAsync("Vangplaats is leeg",
                SnackbarColor.Info,
                options => options.IntervalBeforeClose = NOTIFICATION_SHOW_TIME
            );

            return false;
        }

        var catchLocation = new CatchOrigin
        {
            Name = CatchOriginName,
            Type = _selections.CatchOriginType
        };

        var createSuperkatParameters = new CreateSuperkatParameters()
        {
            CatchDate = CatchDate ?? DateTime.UtcNow,
            CatchOrigin = catchLocation,
            CatArea = _selections.CatArea,
            CageNumber = _selections.CageNumber,
            Behaviour = _selections.CatBehaviour,
            Retour = Retour,
            AgeCategory = _selections.AgeCategory,
            Gender = _selections.Gender,
            LitterType = _selections.LitterGranuleType,
            WetFoodAllowed = WetFoodAllowed,
            FoodType = _selections.FoodType,
            CatColor = CatColor,
            EstimatedWeeksOld = EstimatedWeeksOld,
            StrongholdGiven = StrongHoldGiven
        };

        try
        {
            var superkat = await SuperkattenService.CreateSuperkatAsync(createSuperkatParameters);

            if (superkat is null)
            {
                await _snackbarStack!.PushAsync($"Fout bij het opslaan van de gegevens",
                SnackbarColor.Danger,
                options =>
                {
                    options.IntervalBeforeClose = NOTIFICATION_SHOW_TIME;
                });
                return false;
            }

            await _snackbarStack!.PushAsync($"Superkat toegevoegd met nummer: {superkat.UniqueNumber}",
                SnackbarColor.Info,
                options =>
                {
                    options.IntervalBeforeClose = NOTIFICATION_SHOW_TIME;
                });
        }
        catch (Exception ex)
        {
            await _snackbarStack!.PushAsync($"Fout bij het opslaan van de gegevens",
            SnackbarColor.Danger,
            options =>
            {
                options.IntervalBeforeClose = NOTIFICATION_SHOW_TIME;
            });
            return false;
        }

        return true;
    }
}


using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.ApiInterface;
using Superkatten.Katministratie.Contract.ApiInterface.Reallocate;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Interfaces;

namespace Superkatten.Katministratie.Host.Pages.SuperkatPages;

partial class MoveSuperkat
{
    [Inject] private ISettingsService SettingsService { get; set; } = null!;
    [Inject] private Navigation _navigation { get; set; } = null!;
    [Inject] private ISuperkattenListService _superkattenListService { get; set; } = null!;

    [ParameterAttribute]
    public Guid? SuperkatId { get; set; }

    private Superkat? InitialSuperkat { get; set; } = null!;

    private IReadOnlyCollection<Superkat> _superkatten = null!;
    private IReadOnlyCollection<string> _superkatNames = null!;
    private static List<CatArea> _catAreas = null!;
    private static List<string> _catAreaNames = null!;
    private static IReadOnlyCollection<int> _cageNumbers = Array.Empty<int>();
    private static IReadOnlyCollection<string> _cageNumberNames = Array.Empty<string>();

    private bool _forceSelectedSuperkat = false;
    private CatArea _selectedCatArea;
    private int _selectedCageNumber;
    private Superkat? _selectedSuperkat = null;

    private bool IsSelectionComplete => _selectedSuperkat is not null && _selectedCageNumber > 0;
    private string SuperkatUniqueNumber => _selectedSuperkat?.UniqueNumber ?? "???";
    protected override async Task OnInitializedAsync()
    {
        _catAreas = Enum.GetValues(typeof(CatArea)).Cast<CatArea>().ToList();
        _catAreaNames = _catAreas.Select(x => x.ToString()).ToList();

        await SetOrderedSuperkattenListAsync();
        SetInitialSuperkat();
    }

    private async Task SetOrderedSuperkattenListAsync()
    {
        var superkatten = await _superkattenListService.GetAllSuperkattenAsync();
        if (superkatten is null)
        {
            return;
        }

        _superkatten = superkatten
            .OrderBy(s => s.UniqueNumber)
            .ToList();

        var superkatNames = _superkatten
            .Select(s => s.UniqueNumber)
            .ToList();

        _superkatNames = superkatNames ?? Array.Empty<string>().ToList();
    }

    private void SetInitialSuperkat()
    {
        if (SuperkatId is null)
        {
            InitialSuperkat = _superkatten.FirstOrDefault();
            return;
        }

        _forceSelectedSuperkat = true;
        var initialSuperkat = _superkatten
            .Where(s => s.Id == SuperkatId)
            .FirstOrDefault();

        _selectedSuperkat = initialSuperkat;
        InitialSuperkat = initialSuperkat;
    }

    private void OnSelectSuperkat(Superkat superkat)
    {
        _selectedSuperkat = superkat;
    }

    private async Task OnSelectCatArea(CatArea catArea)
    {
        _selectedCatArea = catArea;
        await UpdateCageNumberList();
    }

    private async Task UpdateCageNumberList()
    {
        _cageNumbers = Array.Empty<int>();

        var cageNumbers = await SettingsService.GetCageNumbersForCatAreaAsync(_selectedCatArea);
        _cageNumbers = cageNumbers is null
            ? Array.Empty<int>()
            : cageNumbers;

        var cageNumberNames = _cageNumbers.Select(x => x.ToString()).ToList();
        _cageNumberNames = cageNumberNames;
    }

    private Task OnSelectCageNumber(int cageNumber)
    {
        _selectedCageNumber = cageNumber;

        return Task.CompletedTask;
    }

    private void OnCancel()
    {
        _navigation.NavigateBack();
    }

    private async Task OnOk()
    {
        if (_selectedSuperkat is null)
        {
            return;
        }

        var updateSuperkat = new ReallocateInRefugeParameters()
        {
            SuperkatId = _selectedSuperkat.Id,
            CatArea = _selectedCatArea,
            CageNumber = _selectedCageNumber
        };

        await _superkattenListService.ReallocateSuperkatAsync(updateSuperkat);

        _navigation.NavigateBack();
    }
}
using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Entities;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.LocalStorage;
using Superkatten.Katministratie.Host.Services;

namespace Superkatten.Katministratie.Host.Pages.SuperkatPages;

public partial class OverviewSuperkatten
{
    [Inject] private Navigation _navigation { get; set; } = null!;
    [Inject] private ISuperkattenListService _superkattenService { get; set; } = null!;
    [Inject] private ILocalStorageService _localStorageService { get; set; } = null!;


    private List<Superkat> Superkatten { get; set; } = new();

    private bool _showSimpleListView = false;
    private readonly int _itemsPerPage = 10;
    private readonly int _displayedPageSetCount = 10;
    private int _currentPage = 1;
    private int _totalPageCount = 10;
    private int _startPageNumber = 1;

    private bool IsActive(int pageNumber) => _currentPage == pageNumber;
    private bool IsFirstPageSet => _startPageNumber == 1;
    private int _maxDisplayedPageNumber;
    private bool IsLastPageSet => _maxDisplayedPageNumber > _totalPageCount;
    private bool IsFirstPage => _currentPage == 1;
    private bool IsLastPage => _currentPage == _totalPageCount;

    private async Task OnChangeSimpleListViewAsync()
    {
        Superkatten.Clear();
        
        _showSimpleListView = !_showSimpleListView;

        await _localStorageService.SetItemAsync(
            LocalStorageItems.LOCALSTORAGE_SETTING_SUPERKATTENLIST_TYPE,
            _showSimpleListView);

        await UpdateListAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        _showSimpleListView = await _localStorageService
            .GetItemAsync<bool>(LocalStorageItems.LOCALSTORAGE_SETTING_SUPERKATTENLIST_TYPE);
        
        await UpdateListAsync();

        await InitializePaginationAsync();
    }

    private async Task InitializePaginationAsync()
    {
        var superkatten = await _superkattenService.GetAllSuperkattenAsync();
        _totalPageCount = superkatten is null ? 0 : (superkatten.Count / _itemsPerPage) + 1;
        _maxDisplayedPageNumber = CalculateMaxDisplayedPageNumber();
    }

    private async Task UpdateListAsync()
    {
        var superkatten = await _superkattenService.GetAllSuperkattenAsync();
        
        var partialList = superkatten
            .AsQueryable()
            .OrderBy(sk => sk.Number)
            .Skip((_currentPage - 1) * _itemsPerPage)
            .Take(_itemsPerPage)
            .ToList();

        Superkatten = partialList;
    }

    private void OnBackHome()
    {
        _navigation.NavigateTo("/");
    }

    private async Task Previous()
    {
        _currentPage -= 1;
        if (_currentPage < _startPageNumber)
        {
            _startPageNumber -= _displayedPageSetCount;
            _maxDisplayedPageNumber = CalculateMaxDisplayedPageNumber();
        }

        await UpdateListAsync();
    }

    private async Task Next()
    {
        _currentPage += 1;
        if (_currentPage > _maxDisplayedPageNumber)
        {
            _startPageNumber += _displayedPageSetCount;
            _maxDisplayedPageNumber = CalculateMaxDisplayedPageNumber();
        }
        await UpdateListAsync();
    }

    private async Task SetActive(string page)
    {
        _currentPage = int.Parse(page);
        await UpdateListAsync();
    }

    private async Task PreviousPageSet()
    {
        _startPageNumber -= _displayedPageSetCount;
        _currentPage -= _displayedPageSetCount;

        _maxDisplayedPageNumber = CalculateMaxDisplayedPageNumber();

        await UpdateListAsync();
    }

    private async Task NextPageSet()
    {
        _startPageNumber += _displayedPageSetCount;

        _currentPage += _displayedPageSetCount;
        if(_currentPage > _totalPageCount)
        {
            _currentPage = _totalPageCount;
        }
         
        _maxDisplayedPageNumber = CalculateMaxDisplayedPageNumber();

        await UpdateListAsync();
    }

    private int CalculateMaxDisplayedPageNumber()
    {
        var maxDisplayedPageNumber = _startPageNumber + _displayedPageSetCount - 1;

        return maxDisplayedPageNumber > _totalPageCount
            ? _totalPageCount
            : maxDisplayedPageNumber;
    }

}

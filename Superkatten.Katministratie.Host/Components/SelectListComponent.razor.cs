using Microsoft.AspNetCore.Components;
using MoreLinq;
using Superkatten.Katministratie.Contract.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Superkatten.Katministratie.Host.Components;

public partial class SelectListComponent<TItem>
{
    [Parameter]
    public string DefaultText { get;  set; } = string.Empty;

    [Parameter]
    public TItem? InitialSelectedItem { get;  set; }

    [Parameter]
    public string EmptyListText { get; set; } = "...";

    [Parameter]
    public IReadOnlyCollection<string> ItemNames { get; set; } = Array.Empty<string>();

    [Parameter]
    public IReadOnlyCollection<TItem> Items
    {
        get
        {
            return _items;
        }

        set
        {
            if (value is null)
            {
                return;
            }

            _items = value.ToList();

            SetDefaultItemNames();
            UpdateItems();
        }
    }

    [Parameter]
    public EventCallback<TItem> OnSelectionChanged { get; set; }

    private int _selectedValueIndex = 0;
    private List<TItem> _items = Array.Empty<TItem>().ToList();
    private IEnumerable<SelectListItem<TItem>> _selectionItemData = Array.Empty<SelectListItem<TItem>>();

    protected async override Task OnInitializedAsync()
    {
        if (ItemNames is null || Items is null)
        {
            return;
        }

        var selectedValueIndex = InitialSelectedItem is null
            ? 0
            : GetInitialIndex(InitialSelectedItem);

        await OnSelectedValueChanged(selectedValueIndex);
    }

    private void SetDefaultItemNames()
    {
        if (!ItemNames.Any() || ItemNames.Count != Items.Count)
        {
            ItemNames = _items.Select(x => x?.ToString() ?? string.Empty).ToList();
        }
    }

    private void UpdateItems()
    { 
        _selectionItemData = _items
            .Select((itemObject, index) =>
            {
                return new SelectListItem<TItem>
                {
                    KeyId = index + 1,
                    ItemName = ItemNames.ElementAt(index),
                    Item = itemObject
                };
            })
            .ToList();
    }

    private async Task OnSelectedValueChanged(int selectedListIndex)
    {
        if (selectedListIndex > 0)
        {
            // index 0 is actually the remark placeholder of the SelectList component
            // therefore the selected list index == the item key
            var item = GetSelectedItem(selectedListIndex);
            await OnSelectionChanged.InvokeAsync(item);
        }

        _selectedValueIndex = selectedListIndex;
    }

    private TItem? GetSelectedItem(int keyId)
    {
        var selectedListItem = _selectionItemData
            .Where(x => x.KeyId == keyId)
            .FirstOrDefault();

        return selectedListItem is null || selectedListItem.Item is null
            ? default
            : selectedListItem.Item;
    }

    private int GetInitialIndex(TItem? item)
    {
        return item is null
            ? 0
            : _items.FindIndex(i => i?.Equals(item) ?? false) + 1;
    }
}

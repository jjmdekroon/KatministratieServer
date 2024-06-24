using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Superkatten.Katministratie.Host.Helpers;

public class Navigation : IDisposable
{
    private const int MINIMAL_HISTORY_SIZE = 256;
    private const int ADDITIONAL_HISTORY_SIZE = 64;

    private readonly NavigationManager _navigationManager;
    private readonly List<string> _history = new(MINIMAL_HISTORY_SIZE + ADDITIONAL_HISTORY_SIZE);
    public bool CanNavigateBack => _history.Count >= 2;

    public Navigation(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        
        _history.Add(_navigationManager.Uri);

        _navigationManager.LocationChanged += OnLocationChanged;
    }

    public void NavigateTo(string url)
    {
        _navigationManager.NavigateTo(url);
    }

    public void NavigateBack()
    {
        var backPageUrl = "/";

        if (CanNavigateBack)
        {
            backPageUrl = _history[^2];
            _history.RemoveRange(_history.Count - 2, 2);
        }

        _navigationManager.NavigateTo(backPageUrl);
    }

    public void Reset()
    {
        _history.Clear();
        _history.Add(_navigationManager.Uri);
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        EnsureSize();
        _history.Add(e.Location);
    }

    private void EnsureSize()
    {
        if (_history.Count < MINIMAL_HISTORY_SIZE + ADDITIONAL_HISTORY_SIZE)
        {
            return;
        }

        _history.RemoveRange(0, _history.Count - MINIMAL_HISTORY_SIZE);
    }

    public void Dispose()
    {
        _navigationManager.LocationChanged -= OnLocationChanged;
    }
}

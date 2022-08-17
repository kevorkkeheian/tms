using System.Net.Http.Json;
using Application.Shared.Models.Org;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Application.Client.Services;

public class StateContainer
{
    private string? savedString;

    public string Property
    {
        get => savedString ?? string.Empty;
        set
        {
            savedString = value;
            NotifyStateChanged();
        }
    }

 

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
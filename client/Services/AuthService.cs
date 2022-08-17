using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Services;
using Blazored.LocalStorage;
using Application.Models;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    // public async Task<RegisterResult> Register(RegisterModel registerModel)
    // {
    //     var result = await _httpClient.PostJsonAsync<RegisterResult>("api/accounts", registerModel);

    //     return result;
    // }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        // _httpClient.BaseAddress = new Uri($"https://localhost:7074/");

        // var loginAsJson = JsonSerializer.Serialize(loginModel);
        // var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
        // var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // if (!response.IsSuccessStatusCode)
        // {
        //     return loginResult;
        // }

        // await _localStorage.SetItemAsync("authToken", loginResult.Token);
        
        
        // ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

        // return loginResult;

        var result = await _httpClient.PostJsonAsync<LoginResult>("api/Login", loginModel);

        if (result.Successful)
        {
            await _localStorage.SetItemAsync("authToken", result.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
    
            return result;
        }
    
        return result;
    }

    public async Task Logout()
    {
        _httpClient.BaseAddress = new Uri($"https://localhost:7074/");
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
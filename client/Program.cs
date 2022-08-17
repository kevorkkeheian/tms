using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Application;
using Application.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7074") });

// builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

// // AddHttpClient is an extension in Microsoft.Extensions.Http
// builder.Services.AddHttpClient("WebAPI",
//         client => client.BaseAddress = new Uri("https://localhost:7074"))
//     .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();







builder.Services.AddMudServices();
builder.Services.AddApiAuthorization();

builder.Services.AddApiAuthorization().AddAccountClaimsPrincipalFactory<CustomUserFactory>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddBlazoredLocalStorage();






await builder.Build().RunAsync();

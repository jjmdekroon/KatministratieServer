using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Superkatten.Katministratie.Host;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.LocalStorage;
using Superkatten.Katministratie.Host.Services;
using Superkatten.Katministratie.Host.Services.Authentication;
using Superkatten.Katministratie.Host.Services.Http;
using Superkatten.Katministratie.Host.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Transient services
builder.Services.AddTransient<ISuperkattenListService, SuperkattenListService>();
builder.Services.AddTransient<ISuperkatActionService, SuperkatActionService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<IMedicalProcedureService, MedicalProcedureService>();
builder.Services.AddTransient<IReportingService, ReportingService>();
builder.Services.AddTransient<ICatchOriginService, CatchOriginService>();
builder.Services.AddTransient<ISettingsService, SettingsService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

var urlApi = Environment.GetEnvironmentVariable("APPSETTING_UriSuperkattenApi");
urlApi ??= "https://localhost:5000/";
//urlApi ??= "https://localhost:7171/";

builder.Services.AddTransient(s =>
{
    return new HttpClient { BaseAddress = new Uri(urlApi) };
});

// Add singleton services
builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<IUserLoginService, UserLoginService>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
builder.Services.AddSingleton<Navigation>();

builder.Services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Debug));

builder.Services.AddLocalization();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

await builder
    .Build()
    .RunAsync();

//
// Use configuration appsettings or other config file
// Schemes: https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/understanding-http-authentication

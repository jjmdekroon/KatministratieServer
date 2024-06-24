using Superkatten.Katministratie.Contract.Authenticate;
using Superkatten.Katministratie.Host.Helpers;
using Superkatten.Katministratie.Host.LocalStorage;
using Superkatten.Katministratie.Host.Services.Authentication;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Superkatten.Katministratie.Host.Services.Http;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private Navigation _navigation;
    private readonly ILocalStorageService _localStorageService;

    private readonly IConfiguration _configuration;
    private readonly IUserLoginService _userLoginService;

    public HttpService(
        HttpClient httpClient,
        Navigation navigation,
        ILocalStorageService localStorageService,
        IConfiguration configuration,
        IUserLoginService userLoginService
    )
    {
        _httpClient = httpClient;
        _navigation = navigation;
        _localStorageService = localStorageService;
        _configuration = configuration;
        _userLoginService = userLoginService;
    }

    public async Task<T?> Get<T>(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        return await SendRequest<T>(request);
    }

    public async Task<T?> Get<T>(string uri, object? value = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        if (value is not null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }

        return await SendRequest<T>(request);
    }

    public async Task<T?> Post<T>(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        return await SendRequest<T>(request);
    }

    public async Task<T?> Post<T>(string uri, object value)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        };
        return await SendRequest<T>(request);
    }

    public async Task Post(string uri, object value)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        };
        await SendRequest(request);
    }

    public async Task<T?> Put<T>(string uri, object value)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        };
        return await SendRequest<T>(request);
    }

    public async Task Put(string uri, object value)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json")
        };

        await SendRequest(request);
    }

    public async Task Delete(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);
        await SendRequest(request);
    }

    private async Task SendRequest(HttpRequestMessage request)
    {
        var response = await SendWithAutorisationHeader(request);
        await CheckResponseForErrors(response);
    }

    private async Task<T?> SendRequest<T>(HttpRequestMessage request)
    {
        var response = await SendWithAutorisationHeader(request);
        await CheckResponseForErrors(response);

        return await response.Content.ReadFromJsonAsync<T>();
    }

    private async Task CheckResponseForErrors(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _userLoginService.ResetAsync();
            _navigation.Reset();
            _navigation.NavigateTo("/");
            return;
        }

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            await _userLoginService.ResetAsync();
            _navigation.Reset();
            _navigation.NavigateTo("/");
            return;
        }

        if (!response.IsSuccessStatusCode)
        {
            if (response.Content is null)
            {
                throw new Exception("No content available, reason of failure unknown");
            }

            var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            throw new Exception(
                error is not null
                ? error["message"]
                : "Fatal error"
            );
        }
    }

    private async Task<HttpResponseMessage> SendWithAutorisationHeader(HttpRequestMessage request)
    {
        if (request.RequestUri is null)
        {
            throw new Exception("Request URI is empty");
        }

        // add jwt auth header if user is logged in and request is to the api url
        var user = await _localStorageService.GetItemAsync<AuthenticateResponse>(LocalStorageItems.LOCALSTORAGE_ITEM_USER);
        if (user is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        }
        
        return await _httpClient.SendAsync(request);
    }
}

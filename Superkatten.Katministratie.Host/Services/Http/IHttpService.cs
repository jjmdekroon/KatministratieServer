namespace Superkatten.Katministratie.Host.Services.Http;

public interface IHttpService
{
    Task<T?> Get<T>(string uri);
    Task<T?> Get<T>(string uri, object? value);

    Task<T?> Put<T>(string uri, object value);
    Task Put(string uri, object value);

    Task<T?> Post<T>(string uri);
    Task<T?> Post<T>(string uri, object value);
    Task Post(string uri, object value);

    Task Delete(string uri);
}

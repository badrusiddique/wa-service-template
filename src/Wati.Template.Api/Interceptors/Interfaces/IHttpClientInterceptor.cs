namespace Wati.Template.Api.Interceptors.Interfaces;

public interface IHttpClientInterceptor
{
    ValueTask<string> GetClientIdAsync();
}
using System.Net;
using System.Text;
using EnsureThat;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Wati.Template.Common.Configurations;

namespace Wati.Template.Repository.Repositories;

public static class HttpClientContext
{
    #region Public Methods

    public static async ValueTask<T> GetAsync<T>(this HttpClient client, RequestConfiguration options)
    {
        ValidateOptions(options);
        ParseHeaders(client, options);

        var response = await client.GetAsync(ConstructUri(options));

        return await ParseResponse<T>(response, HttpStatusCode.OK);
    }

    public static async ValueTask<T> SendAsync<T>(this HttpClient client, RequestConfiguration options)
    {
        ValidateSendOptions(options);

        var request = ParseSendRequest(client, options);
        var response = await client.SendAsync(request);

        return await ParseResponse<T>(response, HttpStatusCode.OK);
    }

    public static async ValueTask<T> PostAsync<T>(this HttpClient client, RequestConfiguration options)
    {
        ValidateOptions(options);
        ParseHeaders(client, options);

        var response = options.FormContent == null
            ? client.PostAsync(ConstructUri(options), new StringContent(JsonConvert.SerializeObject(options.Data), Encoding.UTF8, "application/json")).ConfigureAwait(false).GetAwaiter().GetResult()
            : client.PostAsync(ConstructUri(options), options.FormContent).ConfigureAwait(false).GetAwaiter().GetResult();

        return await ParseResponse<T>(response, HttpStatusCode.OK);
    }

    public static Task<T> PutAsync<T>(this HttpClient client, RequestConfiguration options) => throw new NotImplementedException();

    public static ValueTask<T> DeleteAsync<T>(this HttpClient client, RequestConfiguration options) => throw new NotImplementedException();

    #endregion

    #region Private Methods
    private static async Task<T> ParseResponse<T>(HttpResponseMessage response, HttpStatusCode statusCode)
    {
        EnsureArg.IsTrue(
            response.StatusCode == statusCode,
            nameof(response.StatusCode),
            o => o.WithMessage(GetErrorMessage(response)));

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(jsonString);
    }
    private static string GetErrorMessage(HttpResponseMessage response)
    {
        var uri = response.RequestMessage?.RequestUri?.AbsolutePath;
        var statusCode = response.StatusCode.ToString();
        var message = response.ReasonPhrase;
        return $"Error calling: {uri} with status code: {statusCode} and message: {message}";
    }

    private static string ConstructUri(RequestConfiguration options) =>
        options.QueryParameters == null
            ? options.Uri
            : QueryHelpers.AddQueryString(options.Uri, options.QueryParameters);

    private static void ParseHeaders(HttpClient client, RequestConfiguration options)
    {
        if (options?.Headers == null) { return; }

        foreach (var header in options.Headers)
        {
            client.DefaultRequestHeaders.Remove(header.Key);
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }

    private static HttpRequestMessage ParseSendRequest(HttpClient client, RequestConfiguration options)
    {
        ParseHeaders(client, options);

        var request = options.RequestMessage;
        request.RequestUri = new Uri(client.BaseAddress, options.Uri);

        return request;
    }

    private static void ValidateOptions(RequestConfiguration options)
    {
        EnsureArg.IsNotNull(options, nameof(options), o => o.WithMessage($"invalid request options: {options}"));
        EnsureArg.IsNotNullOrEmpty(options.Uri, nameof(options.Uri), o => o.WithMessage($"invalid uri: {options.Uri}"));
    }

    private static void ValidateSendOptions(RequestConfiguration options)
    {
        ValidateOptions(options);

        var request = options.RequestMessage;
        EnsureArg.IsNotNull(request, nameof(request), o => o.WithMessage($"invalid request message: {request}"));
        EnsureArg.IsNotNull(request.Method, nameof(request.Method), o => o.WithMessage($"invalid request message: {request.Method}"));
    }

    #endregion
}
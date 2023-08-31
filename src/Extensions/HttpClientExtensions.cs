using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Zs.Common.Extensions;

public static class HttpClientExtensions
{
    private const string ContentType = "application/json";

    /// <summary>Get specific type result</summary>
    /// <param name="httpClient">Http client</param>
    /// <param name="url">Address without parameters</param>
    /// <param name="queryParams">Query parameters</param>
    /// <param name="cancellationToken">A token that may be used to cancel the read operation</param>
    public static async Task<TResult?> GetAsync<TResult>(
        this HttpClient httpClient,
        string url,
        IDictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var urlWithParams = GetFullUrl(url, queryParams);
        var streamTask = httpClient.GetStreamAsync(urlWithParams, cancellationToken);
        var result = await streamTask.ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<TResult>(result, cancellationToken: cancellationToken);
    }

    /// <summary>Send post request</summary>
    /// <param name="httpClient">Http client</param>
    /// <param name="url">Address without parameters</param>
    /// <param name="postingObject">An object to post</param>
    /// <param name="queryParams">Query parameters</param>
    /// <param name="cancellationToken">A token that may be used to cancel the read operation</param>
    public static async Task<TResult?> PostAsync<TResult>(
        this HttpClient httpClient,
        string url,
        object postingObject,
        IDictionary<string, string>? queryParams = null,
        CancellationToken cancellationToken = default)
    {
        var serializedObject = JsonSerializer.Serialize(postingObject);
        var stringContent = new StringContent(serializedObject, Encoding.UTF8, ContentType);
        var urlWithParams = GetFullUrl(url, queryParams);

        var httpResponse = await httpClient.PostAsync(urlWithParams, stringContent, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();

        var streamTask = httpResponse.Content.ReadAsStreamAsync(cancellationToken);
        var result = await streamTask.ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<TResult>(result, cancellationToken: cancellationToken);
    }

    /// <summary>Send post request</summary>
    /// <param name="httpClient">Http client</param>
    /// <param name="url">Address without parameters</param>
    /// <param name="postingObject">An object to post</param>
    /// <param name="queryParams">Query parameters</param>
    public static async Task PostAsync(
        this HttpClient httpClient,
        string url,
        object postingObject,
        IDictionary<string, string>? queryParams = null)
    {
        var serializedObject = JsonSerializer.Serialize(postingObject);
        var stringContent = new StringContent(serializedObject, Encoding.UTF8, ContentType);
        var urlWithParams = GetFullUrl(url, queryParams);

        var httpResponse = await httpClient.PostAsync(urlWithParams, stringContent).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
    }

    /// <summary> Get URL with parameters </summary>
    /// <param name="url">URL without parameters</param>
    /// <param name="queryParams">Query parameters</param>
    /// <returns>Complete URL with parameters</returns>
    private static string GetFullUrl(string url, IDictionary<string, string>? queryParams)
    {
        if (queryParams == null || !queryParams.Any())
            return url;

        var fullUrlBuilder = new StringBuilder();
        fullUrlBuilder.AppendFormat(!url.Contains('?') ? "{0}?" : "{0}&", url);

        foreach (var p in queryParams)
            fullUrlBuilder.Append($"{p.Key}={p.Value}&");

        return fullUrlBuilder.ToString().TrimEnd('&');
    }
}
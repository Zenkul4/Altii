using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Desktop.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        IgnoreReadOnlyProperties = true,
    };

    public const string BaseUrl = "https://localhost:7220/api";

    public ApiClient()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true // dev only
        };
        _http = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl + "/") };
    }

    public void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearToken()
    {
        _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _http.GetAsync(url);
        await EnsureSuccess(response);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _json)!;
    }

    public async Task<T> PostAsync<T>(string url, object body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body, _json), Encoding.UTF8, "application/json");
        var response = await _http.PostAsync(url, content);
        await EnsureSuccess(response);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _json)!;
    }

    public async Task PatchAsync(string url)
    {
        var response = await _http.PatchAsync(url, null);
        await EnsureSuccess(response);
    }

    public async Task PatchAsync(string url, object body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body, _json), Encoding.UTF8, "application/json");
        var response = await _http.PatchAsync(url, content);
        await EnsureSuccess(response);
    }

    public async Task<T> PatchAsync<T>(string url, string? query = null)
    {
        var fullUrl = query is null ? url : $"{url}?{query}";
        var response = await _http.PatchAsync(fullUrl, null);
        await EnsureSuccess(response);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _json)!;
    }

    public async Task<T> PutAsync<T>(string url, object body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body, _json), Encoding.UTF8, "application/json");
        var response = await _http.PutAsync(url, content);
        await EnsureSuccess(response);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, _json)!;
    }

    private static async Task EnsureSuccess(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            string message;
            try
            {
                var err = JsonSerializer.Deserialize<ApiError>(body, _json);
                message = err?.Message ?? body;
            }
            catch { message = body; }
            throw new ApiException((int)response.StatusCode, message);
        }
    }
}

public class ApiError
{
    public string? Error { get; set; }
    public string? Message { get; set; }
}

public class ApiException : Exception
{
    public int StatusCode { get; }
    public ApiException(int statusCode, string message) : base(message)
        => StatusCode = statusCode;
}
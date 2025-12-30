using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public abstract class ApiTestBase
{
    protected readonly HttpClient _client;
    protected readonly string _baseUrl = "http://localhost:5026"; // ajusta al puerto de tu API

    protected ApiTestBase()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(_baseUrl);
    }

    protected async Task<string?> LoginAndGetTokenAsync(string username, string password)
    {
        var payload = new
        {
            username,
            password
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _client.PostAsync("/api/auth/login", content);
        if (!response.IsSuccessStatusCode)
            return null;

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        if (!doc.RootElement.TryGetProperty("token", out var tokenElement))
            return null;

        return tokenElement.GetString();
    }

    protected void SetBearerToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    protected void ClearAuth()
    {
        _client.DefaultRequestHeaders.Authorization = null;
    }
}
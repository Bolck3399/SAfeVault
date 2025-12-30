using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

public class SecuritySqlInjectionTests : ApiTestBase
{
    [Fact]
    public async Task Login_WithSqlLikeInput_ShouldNotBreakNorBypassAuth()
    {
        var payload = new
        {
            username = "' OR 1=1 --",
            password = "somePassword"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Access_WithMaliciousOwnerParameter_ShouldNotReturnServerError()
    {
        var adminToken = await LoginAndGetTokenAsync("admin", "Admin123!");
        adminToken.Should().NotBeNullOrEmpty();

        SetBearerToken(adminToken!);

        // Aunque tu endpoint actual no recibe parámetros,
        // simulamos una querystring maliciosa en la URL
        var response = await _client.GetAsync("/api/vault/mine?owner=' OR 1=1 --");

        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
    }
}
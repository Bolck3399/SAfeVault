using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

public class AuthAndAuthorizationTests : ApiTestBase
{
    [Fact]
    public async Task Login_WithValidAdminCredentials_ShouldReturnToken()
    {
        var token = await LoginAndGetTokenAsync("admin", "Admin123!");

        token.Should().NotBeNullOrEmpty("admin credentials should produce a valid JWT");
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        var payload = new
        {
            username = "admin",
            password = "WrongPassword!"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task User_ShouldNotAccess_AdminOnly_Endpoint()
    {
        var userToken = await LoginAndGetTokenAsync("user", "User123!");
        userToken.Should().NotBeNullOrEmpty();

        SetBearerToken(userToken!);

        var response = await _client.GetAsync("/api/vault/all");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Admin_ShouldAccess_AdminOnly_Endpoint()
    {
        var adminToken = await LoginAndGetTokenAsync("admin", "Admin123!");
        adminToken.Should().NotBeNullOrEmpty();

        SetBearerToken(adminToken!);

        var response = await _client.GetAsync("/api/vault/all");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UnauthenticatedUser_CannotAccess_ProtectedEndpoint()
    {
        ClearAuth();

        var response = await _client.GetAsync("/api/vault/mine");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
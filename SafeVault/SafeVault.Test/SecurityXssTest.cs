using System.Net;
using FluentAssertions;
using Xunit;

public class SecurityXssTests : ApiTestBase
{
    [Fact]
    public async Task Response_ShouldBeJson_AndNotHtml()
    {
        var adminToken = await LoginAndGetTokenAsync("admin", "Admin123!");
        adminToken.Should().NotBeNullOrEmpty();

        SetBearerToken(adminToken!);

        var response = await _client.GetAsync("/api/vault/mine");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task Api_ShouldHandleSpecialCharacters_WithoutError()
    {
        var adminToken = await LoginAndGetTokenAsync("admin", "Admin123!");
        adminToken.Should().NotBeNullOrEmpty();

        SetBearerToken(adminToken!);

        // En este caso solo comprobamos que el backend no truene
        var response = await _client.GetAsync("/api/vault/mine?test=<script>alert('xss')</script>");

        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
    }
}
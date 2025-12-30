using System.Net;
using FluentAssertions;
using Xunit;

public class ErrorHandlingTests : ApiTestBase
{
    [Fact]
    public async Task InvalidRoute_ShouldReturnNotFound_AndNotExposeStackTrace()
    {
        var response = await _client.GetAsync("/api/this-route-does-not-exist");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().NotContain("StackTrace", "stack traces should not be exposed in production");
    }
}
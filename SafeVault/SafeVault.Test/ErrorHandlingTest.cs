using System.Net;
using System.Threading.Tasks;
using Xunit;

public class ErrorHandlingTests : ApiTestBase
{
    [Fact]
    public async Task InvalidRoute_ShouldReturnNotFound_AndNotExposeStackTrace()
    {
        var response = await _client.GetAsync("/api/this-route-does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("StackTrace", body);
    }
}
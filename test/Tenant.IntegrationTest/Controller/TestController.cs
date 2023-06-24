using Tenant.IntegrationTest.Configuration;
using Xunit;

namespace Tenant.IntegrationTest.Controller;

public class TestController
{
    private readonly CustomWebApplicationFactory _factory;

    public TestController()
    {
        _factory = new CustomWebApplicationFactory();
    }

    [Fact]
    public async Task Test()
    {
        var client = _factory.CreateClient();
        var result = await client.GetAsync("api/hello-world");
        Assert.True(result.IsSuccessStatusCode);
    }
}
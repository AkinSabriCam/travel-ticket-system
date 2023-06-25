using Tenant.IntegrationTest.Configuration;

namespace Tenant.IntegrationTest.Controller;

public class BaseEpTest : IDisposable
{
    protected readonly HttpClient Client;
    private readonly CustomWebApplicationFactory _factory;
    protected BaseEpTest()
    {
        _factory = new CustomWebApplicationFactory();
        Client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}
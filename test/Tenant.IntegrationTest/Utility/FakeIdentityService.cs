using Master.Application.HttpServices;
using Master.Application.HttpServices.RequestModels;

namespace Tenant.IntegrationTest.Utility;

public class FakeIdentityService : IIdentityHttpService
{
    public Task CreateUser(CreateUserRequest request, string password, 
        string tenantId, string tenantCode)
    {
        return Task.CompletedTask;
    }
}
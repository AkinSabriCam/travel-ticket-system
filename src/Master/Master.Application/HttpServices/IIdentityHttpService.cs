using Master.Application.HttpServices.RequestModels;

namespace Master.Application.HttpServices;

public interface IIdentityHttpService
{
    Task CreateUser(CreateUserRequest request, string password, string tenantId, string tenantCode);
}
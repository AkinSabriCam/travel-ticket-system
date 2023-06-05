using Master.Application.Abstraction.Dto;
using Master.Domain.Tenant.Dto;

namespace Master.Application.Abstraction;

public interface ITenantAppService
{
    Task AddTenant(CreateTenantDto dto, Guid tenantId);
    Task AddUser(CreateUserDto dto, Guid userId);
}
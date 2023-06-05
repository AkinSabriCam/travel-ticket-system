using Common.Validation;
using Master.Domain.Tenant.Dto;

namespace Master.Domain.Tenant;

public interface ITenantDomainService
{
    Task<Result<Tenant>> Create(CreateTenantDto dto, Guid userId);
}
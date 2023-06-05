using Common.Validation;
using Master.Domain.Tenant.Dto;

namespace Master.Domain.Tenant;

public class TenantDomainService : ITenantDomainService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantDomainService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Result<Tenant>> Create(CreateTenantDto dto, Guid userId)
    {
        if (await _tenantRepository.IsCodeExist(dto.Code))
            return Result<Tenant>.Fail("The Tenant Code Is Already Exist!");
        
        var tenant = new Tenant()
        {
            Code = dto.Code,
            Name = dto.Name,
            City = dto.City,
            Country = dto.Country,
            Email = dto.Email
        };

        await _tenantRepository.Create(tenant);
        return Result<Tenant>.Ok(tenant);
    }
}
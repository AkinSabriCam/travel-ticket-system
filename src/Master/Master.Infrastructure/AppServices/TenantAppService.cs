using Common;
using Master.Application.Abstraction;
using Master.Application.Abstraction.Dto;
using Master.Domain.Tenant.Dto;
using Tenant.Domain;
using Tenant.Domain.Tenant;
using Tenant.Domain.User;
using IUserRepository = Tenant.Domain.User.IUserRepository;
using TenantDomain = Tenant.Domain.Tenant.Tenant;

namespace Master.Infrastructure.AppServices;

public class TenantAppService : ITenantAppService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITenantUnitOfWork _tenantUnitOfWork;

    public TenantAppService(ITenantRepository tenantRepository, IUserRepository userRepository,
        ITenantUnitOfWork tenantUnitOfWork)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _tenantUnitOfWork = tenantUnitOfWork;
    }

    public async Task AddTenant(CreateTenantDto dto, Guid tenantId)
    {
        await _tenantRepository.Create(new TenantDomain()
        {
            Id = tenantId,
            Code = dto.Code,
            Name = dto.Name,
            Email = dto.Email
        });

        await _tenantUnitOfWork.SaveChangesAsync();
    }

    public async Task AddUser(CreateUserDto dto, Guid userId)
    {
        await _userRepository.Create(new User()
        {
            Id = userId,
            Username = dto.Username,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Password = dto.Password,
            Email = dto.Email,
            Type = UserType.TenantOwner
        });

        await _tenantUnitOfWork.SaveChangesAsync();
    }
}
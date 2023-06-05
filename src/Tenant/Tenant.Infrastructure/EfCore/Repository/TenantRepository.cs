using Tenant.Domain.Tenant;

namespace Tenant.Infrastructure.EfCore.Repository;

public class TenantRepository : BaseRepository<Domain.Tenant.Tenant, Guid>, ITenantRepository
{
    public TenantRepository(TenantDbContext dbContext) : base(dbContext)
    {
    } 
}
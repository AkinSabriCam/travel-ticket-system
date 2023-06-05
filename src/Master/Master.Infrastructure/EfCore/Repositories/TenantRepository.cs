using Master.Domain.Tenant;
using Microsoft.EntityFrameworkCore;

namespace Master.Infrastructure.EfCore.Repositories;

public class TenantRepository : BaseRepository<Domain.Tenant.Tenant, Guid>, ITenantRepository
{
    public TenantRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> IsCodeExist(string code)
    {
        return QueryAsNoTracking().AnyAsync(x => x.Code == code);
    }
}
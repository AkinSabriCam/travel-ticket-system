using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Tenant.Infrastructure.EfCore.DbContextCache;

public class CustomModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context, bool designTime)
        => context is TenantDbContext tenantDbContext
            ? (context.GetType(), tenantDbContext.TenantId, designTime)
            : context.GetType();
}
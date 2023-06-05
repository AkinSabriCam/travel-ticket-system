using Common.DataAccess;

namespace Tenant.Domain.Tenant;

public interface ITenantRepository : IRepository<Tenant, Guid>
{ 
}
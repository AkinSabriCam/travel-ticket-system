using Common.DataAccess;

namespace Master.Domain.Tenant;

public interface ITenantRepository : IRepository<Tenant, Guid>
{
    Task<bool> IsCodeExist(string code);
}
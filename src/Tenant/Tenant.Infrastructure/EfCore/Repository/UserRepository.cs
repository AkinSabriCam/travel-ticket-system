using Tenant.Domain.User;

namespace Tenant.Infrastructure.EfCore.Repository;

public class UserRepository : BaseRepository<Domain.User.User, Guid>, IUserRepository
{
    public UserRepository(TenantDbContext dbContext) : base(dbContext)
    {
    } 
}
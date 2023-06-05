using Common.DataAccess;

namespace Tenant.Domain.User;

public interface IUserRepository : IRepository<User, Guid>
{
    
}
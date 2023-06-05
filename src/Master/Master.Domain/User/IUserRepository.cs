using Common.DataAccess;

namespace Master.Domain.User;

public interface IUserRepository : IRepository<User, Guid>
{
    
}
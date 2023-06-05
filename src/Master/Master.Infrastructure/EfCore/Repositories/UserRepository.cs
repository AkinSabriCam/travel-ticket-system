using Master.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Master.Infrastructure.EfCore.Repositories;

public class UserRepository : BaseRepository<User, Guid>, IUserRepository
{
    public UserRepository(MasterDbContext dbContext) : base(dbContext)
    {
    }
}
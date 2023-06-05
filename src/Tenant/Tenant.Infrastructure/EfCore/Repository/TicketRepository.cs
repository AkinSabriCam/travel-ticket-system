using Tenant.Domain.Ticket;

namespace Tenant.Infrastructure.EfCore.Repository;

public class TicketRepository : BaseRepository<Ticket, Guid>, ITicketRepository
{
    public TicketRepository(TenantDbContext dbContext) : base(dbContext)
    {
    }
}
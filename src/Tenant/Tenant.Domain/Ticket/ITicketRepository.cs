using Common.DataAccess;

namespace Tenant.Domain.Ticket;

public interface ITicketRepository : IRepository<Ticket, Guid>
{
    
}
using Common.Validation;
using Tenant.Domain.Ticket.Dtos;

namespace Tenant.Domain.Ticket;

public class TicketDomainService : ITicketDomainService
{
    public Task<Result<Ticket>> Create(CreateTicketDto createDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Purchase(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Cancel(Guid id)
    {
        throw new NotImplementedException();
    }
}
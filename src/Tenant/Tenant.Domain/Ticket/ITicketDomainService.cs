using Common.Validation;
using Tenant.Domain.Ticket.Dtos;

namespace Tenant.Domain.Ticket;

public interface ITicketDomainService
{
    Task<Result<Ticket>> Create(CreateTicketDto createDto);
    Task<Result> Purchase(Guid id);
    Task<Result> Cancel(Guid id);
}
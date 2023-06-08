using MediatR;
using Tenant.Application.Queries.Ticket.GetAllTickets;

namespace Tenant.Application.Queries.Ticket.GetTicketById;

public class GetTicketByIdQuery : IRequest<TicketDto>
{
    public Guid Id { get; }

    public GetTicketByIdQuery(Guid id)
    {
        Id = Id;
    }
}
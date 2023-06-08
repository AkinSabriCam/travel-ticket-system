using MediatR;

namespace Tenant.Application.Commands.Ticket.CancelTicket;

public class CancelTicketCommand : IRequest
{
    public Guid Id { get; }

    public CancelTicketCommand(Guid id)
    {
        Id = id;
    }
}
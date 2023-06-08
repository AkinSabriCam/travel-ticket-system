using MediatR;

namespace Tenant.Application.Commands.Ticket.PurchaseTicket;

public class PurchaseTicketCommand : IRequest
{
    public Guid Id { get; }

    public PurchaseTicketCommand(Guid id)
    {
        Id = id;
    }
}
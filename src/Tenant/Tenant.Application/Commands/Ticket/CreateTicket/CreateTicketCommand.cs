using MediatR;
using Tenant.Application.Queries.Ticket.GetAllTickets;
using Tenant.Domain.Ticket;

namespace Tenant.Application.Commands.Ticket.CreateTicket;

public class CreateTicketCommand : IRequest<TicketDto>
{
    public Guid ExpeditionId { get; set; }
    public Guid PassengerId { get; set; }
    public TicketStatus Status { get; set; }
    public string SeatNumber { get; set; }
}       
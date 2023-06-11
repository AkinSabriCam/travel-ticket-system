namespace Tenant.Domain.Ticket.Dtos;

public class CreateTicketDto
{
    public Guid ExpeditionId { get; set; }
    public Guid PassengerId { get; set; }
    public TicketStatus Status { get; set; }
    public string SeatNumber { get; set; }
}
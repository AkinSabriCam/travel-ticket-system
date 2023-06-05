using Common.Entity;

namespace Tenant.Domain.Ticket;

public class TicketHistory : Entity<Guid>
{
    public Guid TicketId { get; set; }
    public decimal Price { get; set; }
    public DateTime DepartureDate { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
}
using Common.Entity;

namespace Tenant.Domain.Ticket;

public class Ticket : AggregateRoot<Guid>
{
    public Guid? ExpeditionId { get; set; }
    public Guid PassengerId { get; set; }
    public decimal Price { get; set; }
    public DateTime DepartureDate { get; set; }
    public TicketStatus Status { get; set; }
    public List<TicketHistory> History { get; set; }
    public Expedition.Expedition Expedition { get; set; }
    public Passenger.Passenger Passenger { get; set; }

    public void AddHistory()
    {
        History.Add(new TicketHistory()
        {
            Price = Price,
            Status = Status,
            DepartureDate = DepartureDate,
        });
    }
}

public enum TicketStatus
{
    Reserved,
    Purchased,
    Cancelled
}
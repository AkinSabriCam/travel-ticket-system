using Common.Entity;
using Common.Validation;

namespace Tenant.Domain.Ticket;

public class Ticket : AggregateRoot
{
    public Guid? ExpeditionId { get; set; }
    public Guid PassengerId { get; set; }
    public decimal Price { get; set; }
    public string SeatNumber { get; private set; }
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
    
    public Result SetSeatNumber(string seatNumber)
    {
        if (string.IsNullOrEmpty(seatNumber))
            return Result.Fail("Seat no can not be empty!");

        if(seatNumber.Any(char.IsLetter))
            return Result.Fail("Seat no should become numeric value");

        SeatNumber = seatNumber;
        return Result.Ok();
    }
}

public enum TicketStatus
{
    Reserved,
    Purchased,
    Cancelled
}
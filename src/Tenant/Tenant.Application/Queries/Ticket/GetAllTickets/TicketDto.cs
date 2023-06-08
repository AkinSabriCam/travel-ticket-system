using System.Linq.Expressions;
using Tenant.Domain.Ticket;

namespace Tenant.Application.Queries.Ticket.GetAllTickets;

public class TicketDto
{
    public Guid Id { get; set; }
    public decimal Price { get; set; }
    public DateTime DepartureDate { get; set; }
    public TicketStatus Status { get; set; }
    public string ArrivalPoint { get; set; }
    public string DeparturePoint { get; set; }
    public string VehicleNo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
    
    
    public static Expression<Func<Domain.Ticket.Ticket, TicketDto>> GetProjection()
    {
        return x => new TicketDto()
        {
            Id = x.Id,
            Price = x.Price,
            Status = x.Status,
            DepartureDate = x.DepartureDate,
            DeparturePoint = x.Expedition.DeparturePoint,
            ArrivalPoint = x.Expedition.ArrivalPoint,
            VehicleNo = x.Expedition.VehicleNo,
            FirstName = x.Passenger.FirstName,
            LastName = x.Passenger.LastName,
            Identity = x.Passenger.Identity
        };
    }
}
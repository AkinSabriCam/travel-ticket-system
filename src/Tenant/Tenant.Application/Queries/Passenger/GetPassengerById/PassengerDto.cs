using System.Linq.Expressions;

namespace Tenant.Application.Queries.Passenger.GetPassengerById;

public class PassengerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
    public string SeatNumber { get; set; }
    public Guid ExpeditionId { get; set; }
    
    public static Expression<Func<Domain.Passenger.Passenger, PassengerDto>> GetProjection()
    {
        return x => new PassengerDto()
        {
            Id = x.Id,
            Identity = x.Identity,
            FirstName = x.FirstName,
            LastName = x.LastName,
            SeatNumber = x.SeatNumber,
            ExpeditionId = x.ExpeditionId,
        };
    }
}
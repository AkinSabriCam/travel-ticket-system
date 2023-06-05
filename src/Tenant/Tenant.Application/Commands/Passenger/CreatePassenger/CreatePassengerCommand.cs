using MediatR;

namespace Tenant.Application.Commands.Passenger.CreatePassenger;

public class CreatePassengerCommand : IRequest<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
    public Guid ExpeditionId { get; set; }
    public string SeatNumber { get; set; }
}
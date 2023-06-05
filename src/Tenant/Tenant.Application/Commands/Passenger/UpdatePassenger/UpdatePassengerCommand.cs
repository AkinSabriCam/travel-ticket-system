using MediatR;

namespace Tenant.Application.Commands.Passenger.UpdatePassenger;

public class UpdatePassengerCommand : IRequest
{
    public Guid Id { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Identity { get; set; }
    public Guid ExpeditionId { get; set; }
    public string SeatNumber { get; set; }

    public UpdatePassengerCommand SetId(Guid id)
    {
        Id = id;
        return this;
    }
}
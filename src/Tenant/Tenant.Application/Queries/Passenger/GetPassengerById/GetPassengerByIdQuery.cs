using MediatR;

namespace Tenant.Application.Queries.Passenger.GetPassengerById;

public class GetPassengerByIdQuery : IRequest<PassengerDto>
{
    public Guid Id { get; }

    public GetPassengerByIdQuery(Guid id)
    {
        Id = id;
    }
}
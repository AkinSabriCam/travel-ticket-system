using MediatR;
using Tenant.Application.Queries.Passenger.GetPassengerById;

namespace Tenant.Application.Queries.Passenger.GetAllPassengers;

public class GetAllPassengersQuery : IRequest<List<PassengerDto>>
{
    
}
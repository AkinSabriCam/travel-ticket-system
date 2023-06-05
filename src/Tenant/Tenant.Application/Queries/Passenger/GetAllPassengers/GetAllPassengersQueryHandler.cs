using System.Linq.Expressions;
using MediatR;
using Tenant.Application.Queries.Passenger.GetPassengerById;
using Tenant.Domain.Passenger;

namespace Tenant.Application.Queries.Passenger.GetAllPassengers;

public class GetAllPassengersQueryHandler : IRequestHandler<GetAllPassengersQuery, List<PassengerDto>>
{
    private readonly IPassengerRepository _repository;

    public GetAllPassengersQueryHandler(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<List<PassengerDto>> Handle(GetAllPassengersQuery request, CancellationToken cancellationToken)
    {
        return _repository.Get(PassengerDto.GetProjection());
    }
}
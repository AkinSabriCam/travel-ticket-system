using MediatR;
using Tenant.Domain.Passenger;

namespace Tenant.Application.Queries.Passenger.GetPassengerById;

public class GetPassengerByIdQueryHandler : IRequestHandler<GetPassengerByIdQuery, PassengerDto>
{
    private readonly IPassengerRepository _repository;

    public GetPassengerByIdQueryHandler(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<PassengerDto> Handle(GetPassengerByIdQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetById(request.Id,PassengerDto.GetProjection());
    }
}
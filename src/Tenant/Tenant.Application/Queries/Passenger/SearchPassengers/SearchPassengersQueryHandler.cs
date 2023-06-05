using MediatR;
using Tenant.Application.Queries.Passenger.GetPassengerById;
using Tenant.Domain.Passenger;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Application.Queries.Passenger.SearchPassengers;

public class SearchPassengersQueryHandler : IRequestHandler<SearchPassengersQuery, List<PassengerDto>>
{
    private readonly IPassengerRepository _repository;

    public SearchPassengersQueryHandler(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public Task<List<PassengerDto>> Handle(SearchPassengersQuery request, CancellationToken cancellationToken)
    {
        return _repository.Search(new SearchDto(request.Keyword, request.Type, request.IsDesc),
            PassengerDto.GetProjection());
    }
}
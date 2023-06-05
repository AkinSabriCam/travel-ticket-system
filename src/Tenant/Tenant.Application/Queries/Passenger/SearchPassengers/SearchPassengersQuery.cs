using MediatR;
using Tenant.Application.Queries.Passenger.GetPassengerById;
using Tenant.Domain.Passenger;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Application.Queries.Passenger.SearchPassengers;

public class SearchPassengersQuery : IRequest<List<PassengerDto>>
{
    public string Keyword { get; set; }
    public PassengerOrderType Type { get; set; }
    public bool IsDesc { get; set; }
}
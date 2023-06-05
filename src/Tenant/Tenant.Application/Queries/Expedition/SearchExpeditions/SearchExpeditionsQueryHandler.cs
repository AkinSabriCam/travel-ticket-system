using System.Linq.Expressions;
using MediatR;
using Tenant.Application.Queries.Expedition.GetExpeditionById;
using Tenant.Domain.Expedition;

namespace Tenant.Application.Queries.Expedition.SearchExpeditions;

public class SearchExpeditionsQueryHandler : IRequestHandler<SearchExpeditionsQuery, List<ExpeditionDto>>
{
    private readonly IExpeditionRepository _repository;

    public SearchExpeditionsQueryHandler(IExpeditionRepository repository)
    {
        _repository = repository;
    }

    public Task<List<ExpeditionDto>> Handle(SearchExpeditionsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Search(new SearchDto(request.Keyword, request.OrderType, request.IsDesc), GetProjection());
    }

    private static Expression<Func<Domain.Expedition.Expedition, ExpeditionDto>> GetProjection()
    {
        return x => new ExpeditionDto()
        {
            Id = x.Id,
            ArrivalPoint = x.ArrivalPoint,
            DepartureDate = x.DepartureDate,
            DeparturePoint = x.DeparturePoint,
            ExpeditionNo = x.ExpeditionNo,
            UnitPrice = x.UnitPrice,
            VehicleNo = x.VehicleNo
        };
    }
}
using System.Linq.Expressions;
using Common.Cache;
using Common.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Application.Queries.Expedition.GetExpeditionById;
using Tenant.Domain.Expedition;

namespace Tenant.Application.Queries.Expedition.GetAllExpeditions;

public class GetAllExpeditionsQueryHandler : IRequestHandler<GetAllExpeditionsQuery, List<ExpeditionDto>>
{
    private readonly IExpeditionRepository _repository;
    private readonly ICacheService _cacheService;
    
    public GetAllExpeditionsQueryHandler(IExpeditionRepository repository, 
        ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<List<ExpeditionDto>> Handle(GetAllExpeditionsQuery request, CancellationToken cancellationToken)
    {
        return await _cacheService.Get("expeditions", () => _repository.Get(GetProjection()));
    }

    private static Expression<Func<Domain.Expedition.Expedition, ExpeditionDto>> GetProjection()
    {
        return x => new ExpeditionDto
        {
            Id = x.Id,
            ExpeditionNo = x.ExpeditionNo,
            UnitPrice = x.UnitPrice,
            ArrivalPoint = x.ArrivalPoint,
            DepartureDate = x.DepartureDate,
            DeparturePoint = x.DeparturePoint,
            VehicleNo = x.VehicleNo,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate
        };
    }
}
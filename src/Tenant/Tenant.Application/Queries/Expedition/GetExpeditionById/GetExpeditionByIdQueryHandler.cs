using MediatR;
using Tenant.Domain.Expedition;

namespace Tenant.Application.Queries.Expedition.GetExpeditionById;

public class GetExpeditionByIdQueryHandler : IRequestHandler<GetExpeditionByIdQuery, ExpeditionDto>
{
    private readonly IExpeditionRepository _repository;

    public GetExpeditionByIdQueryHandler(IExpeditionRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExpeditionDto> Handle(GetExpeditionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetById(request.Id);
        return new ExpeditionDto()
        {
            Id = entity.Id,
            ExpeditionNo = entity.ExpeditionNo,
            ArrivalPoint = entity.ArrivalPoint,
            DepartureDate = entity.DepartureDate,
            DeparturePoint = entity.DeparturePoint,
            UnitPrice = entity.UnitPrice,
            VehicleNo = entity.VehicleNo
        };
    }
}
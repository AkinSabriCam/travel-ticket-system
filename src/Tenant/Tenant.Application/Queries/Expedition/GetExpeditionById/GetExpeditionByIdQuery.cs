using MediatR;

namespace Tenant.Application.Queries.Expedition.GetExpeditionById;

public class GetExpeditionByIdQuery : IRequest<ExpeditionDto>
{
    public Guid Id { get; }

    public GetExpeditionByIdQuery(Guid id)
    {
        Id = id;
    }
}
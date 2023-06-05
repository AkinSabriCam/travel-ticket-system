using MediatR;
using Tenant.Application.Queries.Expedition.GetExpeditionById;
using Tenant.Domain.Expedition;

namespace Tenant.Application.Queries.Expedition.SearchExpeditions;

public class SearchExpeditionsQuery : IRequest<List<ExpeditionDto>>
{
    public string Keyword { get; set; }
    public bool IsDesc { get; set; }
    public OrderType OrderType { get; set; }
}

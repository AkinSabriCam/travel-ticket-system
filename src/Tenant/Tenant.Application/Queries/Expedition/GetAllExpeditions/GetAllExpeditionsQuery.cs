using MediatR;
using Tenant.Application.Queries.Expedition.GetExpeditionById;

namespace Tenant.Application.Queries.Expedition.GetAllExpeditions;

public class GetAllExpeditionsQuery : IRequest<List<ExpeditionDto>>
{
    
}
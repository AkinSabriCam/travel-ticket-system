using MediatR;

namespace Tenant.Application.Commands.Expedition.DeleteExpedition;

public class DeleteExpeditionCommand : IRequest
{
    public Guid Id { get;}

    public DeleteExpeditionCommand(Guid id)
    {
        Id = id;
    }
}
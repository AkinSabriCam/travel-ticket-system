using Common.Cache;
using MediatR;
using Tenant.Domain;
using Tenant.Domain.Expedition;

namespace Tenant.Application.Commands.Expedition.DeleteExpedition;

public class DeleteExpeditionCommandHandler : IRequestHandler<DeleteExpeditionCommand>
{
    private readonly IExpeditionDomainService _domainService;
    private readonly ICacheService _cacheService;
    private readonly ITenantUnitOfWork _tenantUnitOfWork;

    public DeleteExpeditionCommandHandler(IExpeditionDomainService domainService,
        ITenantUnitOfWork tenantUnitOfWork, ICacheService cacheService)
    {
        _domainService = domainService;
        _tenantUnitOfWork = tenantUnitOfWork;
        _cacheService = cacheService;
    }

    public async Task Handle(DeleteExpeditionCommand request, CancellationToken cancellationToken)
    {
        var result = await _domainService.Delete(request.Id);
        result.ValidateAndThrow();

        await _cacheService.InvalidateKey("expeditions");
        await _tenantUnitOfWork.SaveChangesAsync();
    }
}
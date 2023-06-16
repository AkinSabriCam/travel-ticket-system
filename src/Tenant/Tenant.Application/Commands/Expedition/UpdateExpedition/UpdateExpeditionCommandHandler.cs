using Common.Cache;
using Common.Mapping;
using MediatR;
using Tenant.Application.Commands.Expedition.UpdateExpedition.Notification;
using Tenant.Domain;
using Tenant.Domain.Expedition;
using Tenant.Domain.Expedition.Dtos;

namespace Tenant.Application.Commands.Expedition.UpdateExpedition;

public class UpdateExpeditionCommandHandler : IRequestHandler<UpdateExpeditionCommand>
{
    private readonly IExpeditionDomainService _domainService;
    private readonly ITenantUnitOfWork _tenantUnitOfWork;
    private readonly ICustomMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly IMediator _mediator;
    public UpdateExpeditionCommandHandler(ITenantUnitOfWork tenantUnitOfWork, IExpeditionDomainService domainService,
        ICacheService cacheService, ICustomMapper mapper, IMediator mediator)
    {
        _tenantUnitOfWork = tenantUnitOfWork;
        _domainService = domainService;
        _cacheService = cacheService;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task Handle(UpdateExpeditionCommand request, CancellationToken cancellationToken)
    {
        var result = await _domainService.Update(_mapper.Map<UpdateExpeditionDto>(request));
        result.ValidateAndThrow();
        
        await _tenantUnitOfWork.SaveChangesAsync();
        
        await _cacheService.InvalidateKey("expeditions");
        
        await _mediator.Publish(new UpdatedExpeditionNotification()
        {
            Id = result.Value.Id,
            Changes = result.Value.Changes,
            Type = NotificationType.Updated
        }, cancellationToken);

    }
}
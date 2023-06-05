using Common.Cache;
using Common.Mapping;
using MediatR;
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

    public UpdateExpeditionCommandHandler(ITenantUnitOfWork tenantUnitOfWork, IExpeditionDomainService domainService,
        ICacheService cacheService, ICustomMapper mapper)
    {
        _tenantUnitOfWork = tenantUnitOfWork;
        _domainService = domainService;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task Handle(UpdateExpeditionCommand request, CancellationToken cancellationToken)
    {
        var result = await _domainService.Update(_mapper.Map<UpdateExpeditionDto>(request));
        result.ValidateAndThrow();
        
        await _tenantUnitOfWork.SaveChangesAsync();
        await _cacheService.InvalidateKey("expeditions");
    }
}
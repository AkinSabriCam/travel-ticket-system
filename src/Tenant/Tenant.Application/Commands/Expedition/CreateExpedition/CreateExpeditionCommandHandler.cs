using Common.Cache;
using Common.Mapping;
using MediatR;
using Tenant.Domain;
using Tenant.Domain.Expedition;
using Tenant.Domain.Expedition.Dtos;

namespace Tenant.Application.Commands.Expedition.CreateExpedition;

public class CreateExpeditionCommandHandler : IRequestHandler<CreateExpeditionCommand, Guid>
{
    private readonly IExpeditionDomainService _expeditionDomainService;
    private readonly ICustomMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ITenantUnitOfWork _tenantUnitOfWork;
    
    public CreateExpeditionCommandHandler(ITenantUnitOfWork tenantUnitOfWork, IExpeditionDomainService expeditionDomainService, 
        ICacheService cacheService, ICustomMapper mapper)
    {
        _tenantUnitOfWork = tenantUnitOfWork;
        _expeditionDomainService = expeditionDomainService;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateExpeditionCommand request, CancellationToken cancellationToken)
    {
        var result = await _expeditionDomainService.Create(_mapper.Map<CreateExpeditionDto>(request));
        result.ValidateAndThrow();
        await _cacheService.InvalidateKey("expeditions");
        await _tenantUnitOfWork.SaveChangesAsync();
        return result.Value.Id;
    }
}
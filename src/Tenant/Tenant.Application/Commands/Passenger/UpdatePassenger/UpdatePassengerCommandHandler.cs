using Common.Mapping;
using MediatR;
using Tenant.Domain;
using Tenant.Domain.Passenger;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Application.Commands.Passenger.UpdatePassenger;

public class UpdatePassengerCommandHandler : IRequestHandler<UpdatePassengerCommand>
{
    private readonly IPassengerDomainService _domainService;
    private readonly ICustomMapper _mapper;
    private readonly ITenantUnitOfWork _tenantUnitOfWork;

    public UpdatePassengerCommandHandler(ITenantUnitOfWork tenantUnitOfWork, IPassengerDomainService domainService,
        ICustomMapper mapper)
    {
        _tenantUnitOfWork = tenantUnitOfWork;
        _domainService = domainService;
        _mapper = mapper;
    }

    public async Task Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
    {
        var result = await _domainService.Update(_mapper.Map<UpdatePassengerDto>(request));
        result.ValidateAndThrow();
        await _tenantUnitOfWork.SaveChangesAsync();
    }
}
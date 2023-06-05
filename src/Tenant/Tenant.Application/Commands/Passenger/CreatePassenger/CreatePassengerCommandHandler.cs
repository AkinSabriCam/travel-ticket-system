using Common.Mapping;
using MediatR;
using Tenant.Domain;
using Tenant.Domain.Passenger;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Application.Commands.Passenger.CreatePassenger;

public class CreatePassengerCommandHandler : IRequestHandler<CreatePassengerCommand, Guid>
{
    private readonly IPassengerDomainService _domainService;
    private readonly ICustomMapper _mapper;
    private readonly ITenantUnitOfWork _tenantUnitOfWork;

    public CreatePassengerCommandHandler(IPassengerDomainService domainService, ITenantUnitOfWork tenantUnitOfWork,
        ICustomMapper mapper)
    {
        _domainService = domainService;
        _tenantUnitOfWork = tenantUnitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
    {
        var result = await _domainService.Create(_mapper.Map<CreatePassengerDto>(request));

        result.ValidateAndThrow();
        await _tenantUnitOfWork.SaveChangesAsync();
        return result.Value.Id;
    }
}
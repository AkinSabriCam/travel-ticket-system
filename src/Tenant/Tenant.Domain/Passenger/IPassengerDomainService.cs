using Common.Validation;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Domain.Passenger;

public interface IPassengerDomainService
{
    Task<Result<Passenger>> Create(CreatePassengerDto dto);
    Task<Result<Passenger>> Update(UpdatePassengerDto dto);
}

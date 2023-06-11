using Common.Validation;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Domain.Passenger;

public class PassengerDomainService : IPassengerDomainService
{
    private readonly IPassengerRepository _passengerRepository;

    public PassengerDomainService(IPassengerRepository passengerRepository)
    {
        _passengerRepository = passengerRepository;
    }

    public async Task<Result<Passenger>> Create(CreatePassengerDto dto)
    {
        var entity = new Passenger();
        var result = Result<Passenger>.Ok(entity);
        result.Combine(entity.SetFirstName(dto.FirstName));
        result.Combine(entity.SetLastName(dto.LastName));
        result.Combine(entity.SetIdentity(dto.Identity));

        if (!result.IsSuccess())
            return result;

        await _passengerRepository.Create(entity);
        return result;
    }

    public async Task<Result<Passenger>> Update(UpdatePassengerDto dto)
    {
        var entity = await _passengerRepository.GetById(dto.Id);
        var result = Result<Passenger>.Ok(entity);
        result.Combine(entity.SetFirstName(dto.FirstName));
        result.Combine(entity.SetLastName(dto.LastName));
        result.Combine(entity.SetIdentity(dto.Identity));

        if (result.IsSuccess())
            await _passengerRepository.Update(entity, entity.Id);

        return result;
    }
}
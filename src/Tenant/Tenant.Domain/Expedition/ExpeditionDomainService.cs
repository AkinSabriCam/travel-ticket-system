using Common.Validation;
using Tenant.Domain.Expedition.Dtos;

namespace Tenant.Domain.Expedition;

public class ExpeditionDomainService : IExpeditionDomainService
{
    private readonly IExpeditionRepository _expeditionRepository;

    public ExpeditionDomainService(IExpeditionRepository expeditionRepository)
    {
        _expeditionRepository = expeditionRepository;
    }

    public async Task<Result<Expedition>> Create(CreateExpeditionDto dto)
    {
        var entity = new Expedition();
        var result = Result<Expedition>.Ok(entity);

        var expeditionNo = await _expeditionRepository.GenerateExpeditionNo();
        result.Combine(entity.SetExpeditionNo(expeditionNo));
        result.Combine(entity.SetArrivalPoint(dto.ArrivalPoint));
        result.Combine(entity.SetDeparturePoint(dto.DeparturePoint));
        result.Combine(entity.SetDepartureDate(dto.DepartureDate));
        result.Combine(entity.SetSeatCount(dto.SeatCount));
        result.Combine(entity.SetUnitPrice(dto.UnitPrice));
        result.Combine(entity.SetVehicleNo(dto.VehicleNo));

        if (result.IsSuccess())
            await _expeditionRepository.Create(entity);

        return result;
    }

    public  async Task<Result<Expedition>> Update(UpdateExpeditionDto dto)
    {
        var entity = await _expeditionRepository.GetById(dto.Id);
        var result = Result<Expedition>.Ok(entity);
        
        result.Combine(entity.SetArrivalPoint(dto.ArrivalPoint));
        result.Combine(entity.SetDeparturePoint(dto.DeparturePoint));
        result.Combine(entity.SetDepartureDate(dto.DepartureDate));
        result.Combine(entity.SetSeatCount(dto.SeatCount));
        result.Combine(entity.SetUnitPrice(dto.UnitPrice));
        result.Combine(entity.SetVehicleNo(dto.VehicleNo));

        if (result.IsSuccess())
            await _expeditionRepository.Update(entity, entity.Id);

        return result;
    }

    public async Task<Result> Delete(Guid id)
    {
        var entity = await _expeditionRepository.GetById(id);

        if (entity.Tickets.Any())
            return Result.Fail("You can not delete this expedition! There is one ticket on that expedition at least!");
        
        await _expeditionRepository.Delete(id);
        return Result.Ok();
    }
}
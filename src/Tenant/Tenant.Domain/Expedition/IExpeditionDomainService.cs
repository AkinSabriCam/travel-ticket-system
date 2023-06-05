using Common.Validation;
using Tenant.Domain.Expedition.Dtos;

namespace Tenant.Domain.Expedition;

public interface IExpeditionDomainService
{
    Task<Result<Expedition>> Create(CreateExpeditionDto dto);
    Task<Result<Expedition>> Update(UpdateExpeditionDto dto);
    Task<Result> Delete(Guid id);
}
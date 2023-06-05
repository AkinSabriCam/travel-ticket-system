using System.Linq.Expressions;
using Common.DataAccess;

namespace Tenant.Domain.Expedition;

public interface IExpeditionRepository : IRepository<Expedition, Guid>
{
    Task<List<TDto>> Search<TDto>(SearchDto dto, Expression<Func<Expedition, TDto>> projection) where TDto : class;
    Task<string> GenerateExpeditionNo();
}
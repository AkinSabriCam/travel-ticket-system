using System.Linq.Expressions;
using Common.DataAccess;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Domain.Passenger;

public interface IPassengerRepository : IRepository<Passenger, Guid>
{
    Task<List<TDto>> Search<TDto>(SearchDto dto, Expression<Func<Passenger, TDto>> projection) where TDto : class;
}
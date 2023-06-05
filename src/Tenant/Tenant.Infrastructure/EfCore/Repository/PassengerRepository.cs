using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tenant.Domain.Passenger;
using Tenant.Domain.Passenger.Dtos;

namespace Tenant.Infrastructure.EfCore.Repository;

public class PassengerRepository :BaseRepository<Passenger, Guid>, IPassengerRepository
{
    public PassengerRepository(TenantDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<TDto>> Search<TDto>(SearchDto dto, Expression<Func<Passenger, TDto>> projection) where TDto : class
    {
        var query = QueryAsNoTracking();
        if (!string.IsNullOrWhiteSpace(dto.Keyword))
            query = query.Where(x => x.FirstName.Contains(dto.Keyword) ||
                                     x.LastName.Contains(dto.Keyword) ||
                                     x.Identity.Contains(dto.Keyword));

        query = dto.Type switch
        {
            PassengerOrderType.FirstName => dto.IsDesc
                ? query.OrderByDescending(x => x.FirstName)
                : query.OrderBy(x => x.FirstName),
            PassengerOrderType.LastName => dto.IsDesc
                ? query.OrderByDescending(x => x.LastName)
                : query.OrderBy(x => x.LastName),
            _ => dto.IsDesc
                ? query.OrderByDescending(x => x.CreatedDate)
                : query.OrderBy(x => x.CreatedDate),
        };

        return query.Select(projection).ToListAsync();
    }
}
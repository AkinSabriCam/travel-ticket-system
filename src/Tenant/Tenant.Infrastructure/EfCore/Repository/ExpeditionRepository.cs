using System.Data;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tenant.Domain.Expedition;

namespace Tenant.Infrastructure.EfCore.Repository;

public class ExpeditionRepository : BaseRepository<Expedition, Guid>, IExpeditionRepository
{
    public ExpeditionRepository(TenantDbContext dbContext) : base(dbContext)
    {
    }

    public Task<List<TDto>> Search<TDto>(SearchDto dto, Expression<Func<Expedition, TDto>> projection) where TDto : class
    {
        var query = QueryAsNoTracking();

        if (!string.IsNullOrWhiteSpace(dto.Keyword))
            query = query.Where(x => x.ArrivalPoint.Contains(dto.Keyword) ||
                                     x.DeparturePoint.Contains(dto.Keyword) ||
                                     x.ExpeditionNo.Contains(dto.Keyword) ||
                                     x.VehicleNo.Contains(dto.Keyword));

        query = dto.OrderType switch
        {
            OrderType.VehicleNo => dto.IsDesc
                ? query.OrderByDescending(x => x.VehicleNo)
                : query.OrderBy(x => x.VehicleNo),
            OrderType.ExpeditionNo => dto.IsDesc
                ? query.OrderByDescending(x => x.ExpeditionNo)
                : query.OrderBy(x => x.ExpeditionNo),
            OrderType.DeparturePoint => dto.IsDesc
                ? query.OrderByDescending(x => x.DeparturePoint)
                : query.OrderBy(x => x.DeparturePoint),
            OrderType.ArrivalPoint => dto.IsDesc
                ? query.OrderByDescending(x => x.ArrivalPoint)
                : query.OrderBy(x => x.ArrivalPoint),
            OrderType.DepartureDate => dto.IsDesc
                ? query.OrderByDescending(x => x.DepartureDate)
                : query.OrderBy(x => x.DepartureDate),
            _ => dto.IsDesc ? 
                query.OrderByDescending(x => x.CreatedDate) 
                : query.OrderBy(x => x.CreatedDate)
        };

        return  query.Select(projection).ToListAsync();
    }

    public async Task<string> GenerateExpeditionNo()
    {
        var connection = DbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = $"Select NextVal('expedition_no_sequence')";

        var result = (long)(await command.ExecuteScalarAsync())!;
        return result.ToString();
    }
}
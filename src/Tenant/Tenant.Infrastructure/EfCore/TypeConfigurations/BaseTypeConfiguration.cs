using Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class BaseTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : AggregateRoot
{
    private readonly Guid _tenantId;

    public BaseTypeConfiguration(Guid tenantId)
    {
        _tenantId = tenantId;
    }

    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Ignore(x => x.Changes);
        builder.HasQueryFilter(x => !x.IsDeleted && x.TenantId == _tenantId);
    }
}
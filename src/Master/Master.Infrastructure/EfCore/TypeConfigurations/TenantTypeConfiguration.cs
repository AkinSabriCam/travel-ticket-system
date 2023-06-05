using Master.Domain.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Master.Infrastructure.EfCore.TypeConfigurations;

public class TenantTypeConfiguration : IEntityTypeConfiguration<Domain.Tenant.Tenant>
{
    public void Configure(EntityTypeBuilder<Domain.Tenant.Tenant> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.HasIndex(x => x.Code);
        builder.ToTable("tenants");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class TenantTypeConfiguration : IEntityTypeConfiguration<Domain.Tenant.Tenant>
{
    public void Configure(EntityTypeBuilder<Domain.Tenant.Tenant> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.HasKey(x => x.Code);
        builder.ToTable("tenants");
        builder.HasKey(x => x.Id);
    }
}

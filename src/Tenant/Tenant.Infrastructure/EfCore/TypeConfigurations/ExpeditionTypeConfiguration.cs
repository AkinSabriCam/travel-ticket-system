using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Expedition;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class ExpeditionTypeConfiguration : IEntityTypeConfiguration<Expedition>
{
    private readonly Guid _tenantId;

    public ExpeditionTypeConfiguration(Guid tenantId)
    {
        _tenantId = tenantId;
    }

    public void Configure(EntityTypeBuilder<Expedition> builder)
    {
        builder.HasQueryFilter(x => x.TenantId == _tenantId && !x.IsDeleted);

        builder.ToTable("expeditions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasOne<Domain.User.User>()
            .WithMany().HasForeignKey(x => x.CreatedBy).IsRequired();
        builder.HasOne<Domain.Tenant.Tenant>()
            .WithMany().HasForeignKey(x => x.TenantId).IsRequired();
        builder.HasOne<Domain.User.User>()
            .WithMany().HasForeignKey(x => x.UpdatedBy).IsRequired(false);
    }
}
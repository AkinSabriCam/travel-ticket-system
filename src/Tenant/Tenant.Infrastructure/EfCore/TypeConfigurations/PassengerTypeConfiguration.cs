using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Passenger;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class PassengerTypeConfiguration : IEntityTypeConfiguration<Passenger>
{
    private readonly Guid _tenantId;

    public PassengerTypeConfiguration(Guid tenantId)
    {
        _tenantId = tenantId;
    }

    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.HasQueryFilter(x => x.TenantId == _tenantId && !x.IsDeleted);

        builder.ToTable("passengers");
        builder.HasKey(x => x.Id);
        
        builder.HasOne<Domain.User.User>()
            .WithMany().HasForeignKey(x => x.CreatedBy).IsRequired();
        builder.HasOne<Domain.Tenant.Tenant>()
            .WithMany().HasForeignKey(x => x.TenantId).IsRequired();
        builder.HasOne<Domain.User.User>()
            .WithMany().HasForeignKey(x => x.UpdatedBy).IsRequired(false);
    }
}
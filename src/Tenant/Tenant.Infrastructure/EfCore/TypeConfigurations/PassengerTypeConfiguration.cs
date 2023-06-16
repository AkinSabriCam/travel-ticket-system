using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Passenger;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class PassengerTypeConfiguration : BaseTypeConfiguration<Passenger>
{
    public PassengerTypeConfiguration(Guid tenantId) : base(tenantId)
    {
    }

    public override void Configure(EntityTypeBuilder<Passenger> builder)
    {
        base.Configure(builder);
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
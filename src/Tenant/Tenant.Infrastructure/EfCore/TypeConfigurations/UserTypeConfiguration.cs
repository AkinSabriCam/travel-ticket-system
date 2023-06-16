using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<Domain.User.User>
{
    public void Configure(EntityTypeBuilder<Domain.User.User> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Ignore(x => x.Changes);

        builder.ToTable("users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).HasConversion<string>();
        
        builder.HasOne<Domain.User.User>()
            .WithMany().HasForeignKey(x => x.CreatedBy).IsRequired(false);
        builder.HasOne<Domain.Tenant.Tenant>()
            .WithMany().HasForeignKey(x => x.TenantId).IsRequired();
        builder.HasOne<Domain.User.User>()
            .WithMany().HasForeignKey(x => x.UpdatedBy).IsRequired(false);
    }
}
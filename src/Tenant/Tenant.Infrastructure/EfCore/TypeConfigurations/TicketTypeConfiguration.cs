using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Expedition;
using Tenant.Domain.Passenger;
using Tenant.Domain.Ticket;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class TicketTypeConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasOne<Passenger>()
            .WithMany(x => x.Tickets).HasForeignKey(x => x.PassengerId);

        builder.HasOne<Expedition>()
            .WithMany(x => x.Tickets).HasForeignKey(x => x.ExpeditionId).IsRequired(false);

        builder.OwnsMany(x => x.History, y =>
        {
            y.ToTable("ticket_history");
            y.Property(x => x.Status).HasConversion<string>();
            y.HasOne<Ticket>().WithMany(x => x.History).HasForeignKey(x => x.TicketId);
        });
    }
}
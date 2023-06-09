﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenant.Domain.Expedition;
using Tenant.Domain.Passenger;
using Tenant.Domain.Ticket;

namespace Tenant.Infrastructure.EfCore.TypeConfigurations;

public class TicketTypeConfiguration : BaseTypeConfiguration<Ticket>
{
    public TicketTypeConfiguration(Guid tenantId) : base(tenantId)
    {
    }

    public override void Configure(EntityTypeBuilder<Ticket> builder)
    {
        base.Configure(builder);
        builder.ToTable("tickets");
        builder.HasKey(x => x.Id);
 
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasOne(x => x.Passenger)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.PassengerId);

        builder.HasOne(x => x.Expedition)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.ExpeditionId).IsRequired(false);

        builder.OwnsMany(x => x.History, y =>
        {
            y.ToTable("ticket_history");
            y.Property(x => x.Status).HasConversion<string>();
            y.HasOne<Ticket>().WithMany(x => x.History).HasForeignKey(x => x.TicketId);
        });
    }
}
using Common.Entity;
using Common.User;
using Microsoft.EntityFrameworkCore;
using Tenant.Infrastructure.EfCore.TypeConfigurations;

namespace Tenant.Infrastructure.EfCore;

public class TenantDbContext : DbContext
{
    private readonly IUser _currentUser;
    public Guid TenantId;
    public TenantDbContext(DbContextOptions<TenantDbContext> options, IUser user) : base(options)
    {
        _currentUser = user;
        TenantId = user.TenantId;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
       // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpeditionTypeConfiguration).Assembly);

       modelBuilder.HasSequence<int>("expedition_no_sequence");
       modelBuilder.ApplyConfiguration(new ExpeditionTypeConfiguration(TenantId));
       modelBuilder.ApplyConfiguration(new PassengerTypeConfiguration(TenantId));
       modelBuilder.ApplyConfiguration(new TenantTypeConfiguration());
       modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
       modelBuilder.ApplyConfiguration(new TicketTypeConfiguration());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.Now;
                entry.Entity.CreatedBy = _currentUser.UserId;
                entry.Entity.TenantId = _currentUser.TenantId;
            }
            else
            {
                entry.Entity.UpdatedDate = DateTime.Now;
                entry.Entity.UpdatedBy = _currentUser.UserId;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
using Master.Infrastructure.EfCore.TypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Master.Infrastructure.EfCore;

public class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("master");
        modelBuilder.ApplyConfiguration(new TenantTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
    }
}
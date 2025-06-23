using System.Reflection;
using EfCompositeQueryBug.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCompositeQueryBug;

public class AppDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public DbSet<EntityBase> BaseEntities => Set<EntityBase>();

    public DbSet<EntityA> EntitiesA => Set<EntityA>();
    public DbSet<EntityB> EntitiesB => Set<EntityB>();
    public DbSet<EntityC> EntitiesC => Set<EntityC>();

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntityBase>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
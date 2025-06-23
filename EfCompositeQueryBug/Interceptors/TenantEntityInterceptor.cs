using EfCompositeQueryBug.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfCompositeQueryBug.Interceptors;

public class TenantEntityInterceptor : SaveChangesInterceptor
{
    private readonly ITenantContext _tenantContext;

    public TenantEntityInterceptor(ITenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var tenantEntities = dbContext.ChangeTracker.Entries<ITenantEntity>();

        foreach (var entity in tenantEntities)
        {
            var property = entity.Property(x => x.TenantId);

            if (entity.State == EntityState.Added)
            {
                property.CurrentValue = _tenantContext.TenantId;
            }

            if (entity.State == EntityState.Modified && property.OriginalValue != property.CurrentValue)
            {
                throw new InvalidOperationException($"Updating {nameof(ITenantEntity.TenantId)} after entity is creation is prohibited");
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
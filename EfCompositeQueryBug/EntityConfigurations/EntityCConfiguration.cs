using EfCompositeQueryBug.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCompositeQueryBug.EntityConfigurations;

public class EntityCConfiguration : IEntityTypeConfiguration<EntityC>
{
    public void Configure(EntityTypeBuilder<EntityC> builder)
    {
        builder.ComplexProperty(x => x.NestedProperty, cfg => cfg.ToJson());
    }
}
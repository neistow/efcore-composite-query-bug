using EfCompositeQueryBug.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCompositeQueryBug.EntityConfigurations;

public class EntityBaseConfiguration : IEntityTypeConfiguration<EntityBase>
{
    public void Configure(EntityTypeBuilder<EntityBase> builder)
    {
        builder
            .Property(x => x.EntityType)
            .HasConversion<string>();

        builder.HasIndex(x => x.EntityType);

        builder
            .HasDiscriminator(x => x.EntityType)
            .HasValue<EntityA>(EntityType.A)
            .HasValue<EntityB>(EntityType.B)
            .HasValue<EntityC>(EntityType.C);
    }
}
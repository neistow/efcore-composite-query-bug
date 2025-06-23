namespace EfCompositeQueryBug.Entities;

public class EntityB : EntityBase
{
    public required long LongProperty { get; set; }

    public override EntityType EntityType { get; protected set; } = EntityType.B;
}
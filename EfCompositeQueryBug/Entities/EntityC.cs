namespace EfCompositeQueryBug.Entities;

public class EntityC : EntityB
{
    public override EntityType EntityType { get; protected set; } = EntityType.C;
    public NestedEntity? NestedProperty { get; set; }
}

public class NestedEntity
{
    public required string PropertyA { get; set; }
    public required long PropertyB { get; set; }
}
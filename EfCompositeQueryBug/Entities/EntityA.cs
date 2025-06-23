namespace EfCompositeQueryBug.Entities;

public class EntityA : EntityBase
{
    public override EntityType EntityType { get; protected set; } = EntityType.A;
}
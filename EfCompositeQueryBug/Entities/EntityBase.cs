namespace EfCompositeQueryBug.Entities;

public abstract class EntityBase : ITenantEntity
{
    public Guid Id { get; set; }

    public required Guid GuidProperty { get; set; }

    public Guid? ParentId { get; set; }
    public EntityA? Parent { get; set; }

    public abstract EntityType EntityType { get; protected set; }
    
    public Guid TenantId { get; set; }
}
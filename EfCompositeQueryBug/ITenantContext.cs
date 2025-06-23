namespace EfCompositeQueryBug;

public interface ITenantContext
{
    public Guid TenantId { get; }
}

public class StubTenantContext : ITenantContext
{
    public Guid TenantId => Guid.Parse("5be66826-857e-4fe0-9e7a-624803204279");
}
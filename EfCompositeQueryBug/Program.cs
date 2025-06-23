using EfCompositeQueryBug;
using EfCompositeQueryBug.Entities;
using EfCompositeQueryBug.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ITenantContext, StubTenantContext>();

builder.Services.AddSingleton<TenantEntityInterceptor>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>((sp, b) =>
{
    b.UseSqlServer(connectionString)
        .AddInterceptors(
            sp.GetRequiredService<TenantEntityInterceptor>()
        );
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

var tenantId = Guid.Parse("5be66826-857e-4fe0-9e7a-624803204279");
var guidProperty = Guid.CreateVersion7();

var a1 = new EntityA
{
    TenantId = tenantId,
    GuidProperty = guidProperty,
};
ctx.EntitiesA.Add(a1);

var b1 = new EntityB
{
    LongProperty = 10,
    TenantId = tenantId,
    Parent = a1,
    GuidProperty = guidProperty
};
ctx.EntitiesB.Add(b1);

var a2 = new EntityA
{
    TenantId = tenantId,
    Parent = a1,
    GuidProperty = guidProperty
};
ctx.EntitiesA.Add(a2);

var b2 = new EntityB
{
    LongProperty = 10,
    TenantId = tenantId,
    Parent = a2,
    GuidProperty = guidProperty
};
ctx.EntitiesB.Add(b2);

await ctx.SaveChangesAsync();

ctx.ChangeTracker.Clear();

var items = await ctx.BaseEntities.FromSql(
    $"""
        with query as (select *
                       from BaseEntities
                       where GuidProperty = {guidProperty}
                         and ParentId = {a1.Id}
                       union all
                       select entity.*
                       from BaseEntities entity
                                join query q on q.Id = entity.ParentId)
        select * from query
     """).IgnoreQueryFilters().AsNoTracking().ToListAsync();
Console.WriteLine(items.Count);
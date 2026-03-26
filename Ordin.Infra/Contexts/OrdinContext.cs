using Ordin.Domain.Entities;
using Ordin.Infra.Configurations;
using Ordin.Infra.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ordin.Infra.Contexts;

public class OrdinContext(DbContextOptions<OrdinContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations for all entities that inherit from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
        {
            var configType = typeof(BaseEntityConfiguration<>).MakeGenericType(entityType.ClrType);
            dynamic config = Activator.CreateInstance(configType)!;
            modelBuilder.ApplyConfiguration(config);
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Seed();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
        .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        // Update the UpdatedAt property for all modified or added entities
        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTimeOffset.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
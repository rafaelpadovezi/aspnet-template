using AspnetTemplate.Core.Models;
using Microsoft.EntityFrameworkCore;
using Ziggurat.SqlServer;

namespace AspnetTemplate.Core.Database;

public class AppDbContext : DbContext
{
    public DbSet<Sample> Samples { get; set; }
    public DbSet<Product> Products => Set<Product>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(256);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Product>()
            .OwnsOne(product => product.Photos, builder => builder.ToJson())
            .HasIndex(x => x.Code)
            .IsUnique();

        modelBuilder.MapMessageTracker();
    }

    public override int SaveChanges()
    {
        AutomaticallyAddCreatedAndUpdatedAt();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AutomaticallyAddCreatedAndUpdatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AutomaticallyAddCreatedAndUpdatedAt()
    {
        var entitiesOnDbContext = ChangeTracker.Entries<Entity>();

        foreach (var item in entitiesOnDbContext.Where(t => t.State == EntityState.Added))
        {
            item.Entity.CreatedAt = DateTime.Now;
            item.Entity.UpdatedAt = DateTime.Now;
        }

        foreach (var item in entitiesOnDbContext.Where(t => t.State == EntityState.Modified))
        {
            item.Entity.UpdatedAt = DateTime.Now;
        }
    }
}

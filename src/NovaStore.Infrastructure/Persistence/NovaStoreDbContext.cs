using Microsoft.EntityFrameworkCore;
using NovaStore.Domain.Entities;

namespace NovaStore.Infrastructure.Persistence;

public class NovaStoreDbContext : DbContext
{
  public NovaStoreDbContext(DbContextOptions<NovaStoreDbContext> options)
      : base(options)
  {
  }

  public DbSet<Product> Products => Set<Product>();

  public DbSet<Category> Categories => Set<Category>();

  public DbSet<Brand> Brands => Set<Brand>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(
        typeof(NovaStoreDbContext).Assembly);
  }
}
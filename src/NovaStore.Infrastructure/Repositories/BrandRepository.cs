using Microsoft.EntityFrameworkCore;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Domain.Entities;
using NovaStore.Infrastructure.Persistence;

namespace NovaStore.Infrastructure.Repositories;

public sealed class BrandRepository : IBrandRepository
{
  private readonly NovaStoreDbContext _dbContext;

  public BrandRepository(NovaStoreDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public Task<bool> ExistsBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default)
  {
    return _dbContext.Brands
        .AnyAsync(
            brand => brand.Slug == slug,
            cancellationToken);
  }

  public async Task AddAsync(
      Brand brand,
      CancellationToken cancellationToken = default)
  {
    await _dbContext.Brands.AddAsync(
        brand,
        cancellationToken);
  }

  public async Task SaveChangesAsync(
      CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
using Microsoft.EntityFrameworkCore;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Domain.Entities;
using NovaStore.Infrastructure.Persistence;

namespace NovaStore.Infrastructure.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
  private readonly NovaStoreDbContext _dbContext;

  public CategoryRepository(NovaStoreDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public Task<Category?> GetByIdAsync(
      Guid id,
      CancellationToken cancellationToken = default)
  {
    return _dbContext.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(
            category => category.Id == id,
            cancellationToken);
  }

  public Task<bool> ExistsBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default)
  {
    return _dbContext.Categories
        .AnyAsync(
            category => category.Slug == slug,
            cancellationToken);
  }

  public async Task AddAsync(
      Category category,
      CancellationToken cancellationToken = default)
  {
    await _dbContext.Categories.AddAsync(
        category,
        cancellationToken);
  }

  public async Task SaveChangesAsync(
      CancellationToken cancellationToken = default)
  {
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
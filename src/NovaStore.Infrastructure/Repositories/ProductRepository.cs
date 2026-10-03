using Microsoft.EntityFrameworkCore;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Domain.Entities;
using NovaStore.Infrastructure.Persistence;

namespace NovaStore.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
  private readonly NovaStoreDbContext _dbContext;

  public ProductRepository(NovaStoreDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<IReadOnlyList<Product>> GetAllAsync(
      CancellationToken cancellationToken = default)
  {
    return await _dbContext.Products
        .AsNoTracking()
        .Include(product => product.Category)
        .Include(product => product.Brand)
        .OrderBy(product => product.Name)
        .ToListAsync(cancellationToken);
  }
}
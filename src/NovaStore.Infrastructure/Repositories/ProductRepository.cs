using Microsoft.EntityFrameworkCore;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Domain.Entities;
using NovaStore.Infrastructure.Persistence;

namespace NovaStore.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly NovaStoreDbContext _dbContext;

    public ProductRepository(NovaStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .OrderBy(product => product.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .CountAsync(
                product => product.IsActive,
                cancellationToken);
    }

    public Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .FirstOrDefaultAsync(
                product =>
                    product.Id == id &&
                    product.IsActive,
                cancellationToken);
    }

    public Task<Product?> GetForUpdateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken);
    }

    public Task<bool> ExistsBySlugAsync(
        string slug,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .AnyAsync(
                product => product.Slug == slug,
                cancellationToken);
    }

    public Task<bool> ExistsBySlugExceptIdAsync(
        string slug,
        Guid excludedId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .AnyAsync(
                product =>
                    product.Slug == slug &&
                    product.Id != excludedId,
                cancellationToken);
    }

    public Task<bool> ExistsBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .AnyAsync(
                product => product.Sku == sku,
                cancellationToken);
    }

    public Task<bool> ExistsBySkuExceptIdAsync(
        string sku,
        Guid excludedId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Products
            .AnyAsync(
                product =>
                    product.Sku == sku &&
                    product.Id != excludedId,
                cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(
            product,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
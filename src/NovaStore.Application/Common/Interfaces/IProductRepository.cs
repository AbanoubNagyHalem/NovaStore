using NovaStore.Domain.Entities;

namespace NovaStore.Application.Common.Interfaces;

public interface IProductRepository
{
  Task<IReadOnlyList<Product>> GetAllAsync(
      CancellationToken cancellationToken = default);

  Task<Product?> GetByIdAsync(
      Guid id,
      CancellationToken cancellationToken = default);

  Task<Product?> GetForUpdateAsync(
      Guid id,
      CancellationToken cancellationToken = default);

  Task<bool> ExistsBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

  Task<bool> ExistsBySlugExceptIdAsync(
      string slug,
      Guid excludedId,
      CancellationToken cancellationToken = default);

  Task<bool> ExistsBySkuAsync(
      string sku,
      CancellationToken cancellationToken = default);

  Task<bool> ExistsBySkuExceptIdAsync(
      string sku,
      Guid excludedId,
      CancellationToken cancellationToken = default);

  Task AddAsync(
      Product product,
      CancellationToken cancellationToken = default);

  Task SaveChangesAsync(
      CancellationToken cancellationToken = default);
}
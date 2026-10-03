using NovaStore.Domain.Entities;

namespace NovaStore.Application.Common.Interfaces;

public interface IProductRepository
{
  Task<IReadOnlyList<Product>> GetAllAsync(
      CancellationToken cancellationToken = default);
}
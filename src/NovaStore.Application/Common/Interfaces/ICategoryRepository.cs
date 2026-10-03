using NovaStore.Domain.Entities;

namespace NovaStore.Application.Common.Interfaces;

public interface ICategoryRepository
{
  Task<bool> ExistsBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

  Task AddAsync(
      Category category,
      CancellationToken cancellationToken = default);

  Task SaveChangesAsync(
      CancellationToken cancellationToken = default);
}
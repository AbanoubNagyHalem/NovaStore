using NovaStore.Domain.Entities;

namespace NovaStore.Application.Common.Interfaces;

public interface IBrandRepository
{
  Task<bool> ExistsBySlugAsync(
      string slug,
      CancellationToken cancellationToken = default);

  Task AddAsync(
      Brand brand,
      CancellationToken cancellationToken = default);

  Task SaveChangesAsync(
      CancellationToken cancellationToken = default);
}
using NovaStore.Application.Common.Exceptions;
using NovaStore.Application.Common.Interfaces;

namespace NovaStore.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler
{
  private readonly IProductRepository _productRepository;

  public DeleteProductCommandHandler(
      IProductRepository productRepository)
  {
    _productRepository = productRepository;
  }

  public async Task HandleAsync(
      Guid id,
      CancellationToken cancellationToken = default)
  {
    var product = await _productRepository.GetForUpdateAsync(
        id,
        cancellationToken);

    if (product is null)
    {
      throw new NotFoundException(
          $"Product with ID '{id}' was not found.");
    }

    product.IsActive = false;
    product.UpdatedAtUtc = DateTime.UtcNow;

    await _productRepository.SaveChangesAsync(
        cancellationToken);
  }
}
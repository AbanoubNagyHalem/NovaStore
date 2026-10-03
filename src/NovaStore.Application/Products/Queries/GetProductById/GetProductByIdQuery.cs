using NovaStore.Application.Common.Exceptions;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Application.Products.Common;

namespace NovaStore.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQuery
{
  private readonly IProductRepository _productRepository;

  public GetProductByIdQuery(
      IProductRepository productRepository)
  {
    _productRepository = productRepository;
  }

  public async Task<ProductDto> ExecuteAsync(
      Guid id,
      CancellationToken cancellationToken = default)
  {
    var product = await _productRepository.GetByIdAsync(
        id,
        cancellationToken);

    if (product is null)
    {
      throw new NotFoundException(
          $"Product with ID '{id}' was not found.");
    }

    return new ProductDto(
        product.Id,
        product.Name,
        product.Slug,
        product.Description,
        product.Sku,
        product.Price,
        product.CompareAtPrice,
        product.StockQuantity,
        product.IsFeatured,
        product.CategoryId,
        product.Category.Name,
        product.BrandId,
        product.Brand.Name);
  }
}
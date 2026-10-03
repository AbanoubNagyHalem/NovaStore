using NovaStore.Application.Common.Interfaces;
using NovaStore.Application.Products.Common;

namespace NovaStore.Application.Products.Queries.GetProducts;

public sealed class GetProductsQuery
{
  private readonly IProductRepository _productRepository;

  public GetProductsQuery(IProductRepository productRepository)
  {
    _productRepository = productRepository;
  }

  public async Task<IReadOnlyList<ProductDto>> ExecuteAsync(
      CancellationToken cancellationToken = default)
  {
    var products = await _productRepository.GetAllAsync(cancellationToken);

    return products
        .Select(product => new ProductDto(
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
            product.Brand.Name))
        .ToList();
  }
}
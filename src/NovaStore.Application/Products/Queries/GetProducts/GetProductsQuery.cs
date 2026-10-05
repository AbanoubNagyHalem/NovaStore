using NovaStore.Application.Common.Exceptions;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Application.Common.Models;
using NovaStore.Application.Products.Common;

namespace NovaStore.Application.Products.Queries.GetProducts;

public sealed class GetProductsQuery
{
  private const int MaxPageSize = 100;

  private readonly IProductRepository _productRepository;

  public GetProductsQuery(IProductRepository productRepository)
  {
    _productRepository = productRepository;
  }

  public async Task<PagedResult<ProductDto>> ExecuteAsync(
      ProductQueryParameters parameters,
      CancellationToken cancellationToken = default)
  {
    Validate(parameters);

    var products = await _productRepository.GetAllAsync(
        parameters.PageNumber,
        parameters.PageSize,
        cancellationToken);

    var totalCount = await _productRepository.CountAsync(
        cancellationToken);

    var items = products
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

    var totalPages = totalCount == 0
        ? 0
        : (int)Math.Ceiling(
            totalCount / (double)parameters.PageSize);

    return new PagedResult<ProductDto>(
        items,
        parameters.PageNumber,
        parameters.PageSize,
        totalCount,
        totalPages);
  }

  private static void Validate(
      ProductQueryParameters parameters)
  {
    var errors = new Dictionary<string, string[]>();

    if (parameters.PageNumber < 1)
    {
      errors["pageNumber"] =
          ["Page number must be greater than or equal to 1."];
    }

    if (parameters.PageSize < 1)
    {
      errors["pageSize"] =
          ["Page size must be greater than or equal to 1."];
    }
    else if (parameters.PageSize > MaxPageSize)
    {
      errors["pageSize"] =
          [$"Page size must not exceed {MaxPageSize}."];
    }

    if (errors.Count > 0)
    {
      throw new ValidationException(errors);
    }
  }
}
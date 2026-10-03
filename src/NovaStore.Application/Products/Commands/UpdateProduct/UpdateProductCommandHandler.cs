using NovaStore.Application.Common.Exceptions;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Application.Products.Common;

namespace NovaStore.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler
{
  private readonly IProductRepository _productRepository;
  private readonly ICategoryRepository _categoryRepository;
  private readonly IBrandRepository _brandRepository;

  public UpdateProductCommandHandler(
      IProductRepository productRepository,
      ICategoryRepository categoryRepository,
      IBrandRepository brandRepository)
  {
    _productRepository = productRepository;
    _categoryRepository = categoryRepository;
    _brandRepository = brandRepository;
  }

  public async Task<ProductDto> HandleAsync(
      Guid id,
      UpdateProductCommand command,
      CancellationToken cancellationToken = default)
  {
    Validate(command);

    var product = await _productRepository.GetForUpdateAsync(
        id,
        cancellationToken);

    if (product is null)
    {
      throw new NotFoundException(
          $"Product with ID '{id}' was not found.");
    }

    var name = command.Name.Trim();
    var slug = command.Slug.Trim().ToLowerInvariant();
    var sku = command.Sku.Trim().ToUpperInvariant();

    var slugExists =
        await _productRepository.ExistsBySlugExceptIdAsync(
            slug,
            id,
            cancellationToken);

    if (slugExists)
    {
      throw new ConflictException(
          $"A product with the slug '{slug}' already exists.");
    }

    var skuExists =
        await _productRepository.ExistsBySkuExceptIdAsync(
            sku,
            id,
            cancellationToken);

    if (skuExists)
    {
      throw new ConflictException(
          $"A product with the SKU '{sku}' already exists.");
    }

    var category = await _categoryRepository.GetByIdAsync(
        command.CategoryId,
        cancellationToken);

    if (category is null)
    {
      throw new ValidationException(
          new Dictionary<string, string[]>
          {
            ["categoryId"] =
              [
                  "The selected category does not exist."
              ]
          });
    }

    var brand = await _brandRepository.GetByIdAsync(
        command.BrandId,
        cancellationToken);

    if (brand is null)
    {
      throw new ValidationException(
          new Dictionary<string, string[]>
          {
            ["brandId"] =
              [
                  "The selected brand does not exist."
              ]
          });
    }

    product.Name = name;
    product.Slug = slug;
    product.Description =
        string.IsNullOrWhiteSpace(command.Description)
            ? null
            : command.Description.Trim();
    product.Sku = sku;
    product.Price = command.Price;
    product.CompareAtPrice = command.CompareAtPrice;
    product.StockQuantity = command.StockQuantity;
    product.IsFeatured = command.IsFeatured;
    product.CategoryId = command.CategoryId;
    product.BrandId = command.BrandId;
    product.UpdatedAtUtc = DateTime.UtcNow;

    await _productRepository.SaveChangesAsync(
        cancellationToken);

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
        category.Name,
        product.BrandId,
        brand.Name);
  }

  private static void Validate(
      UpdateProductCommand command)
  {
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(command.Name))
    {
      errors["name"] = ["Product name is required."];
    }
    else if (command.Name.Trim().Length > 200)
    {
      errors["name"] =
          ["Product name must not exceed 200 characters."];
    }

    if (string.IsNullOrWhiteSpace(command.Slug))
    {
      errors["slug"] = ["Product slug is required."];
    }
    else if (command.Slug.Trim().Length > 220)
    {
      errors["slug"] =
          ["Product slug must not exceed 220 characters."];
    }

    if (command.Description?.Trim().Length > 4000)
    {
      errors["description"] =
          ["Product description must not exceed 4000 characters."];
    }

    if (string.IsNullOrWhiteSpace(command.Sku))
    {
      errors["sku"] = ["Product SKU is required."];
    }
    else if (command.Sku.Trim().Length > 100)
    {
      errors["sku"] =
          ["Product SKU must not exceed 100 characters."];
    }

    if (command.Price <= 0)
    {
      errors["price"] =
          ["Product price must be greater than zero."];
    }

    if (command.CompareAtPrice.HasValue &&
        command.CompareAtPrice.Value <= command.Price)
    {
      errors["compareAtPrice"] =
      [
          "Compare-at price must be greater than the product price."
      ];
    }

    if (command.StockQuantity < 0)
    {
      errors["stockQuantity"] =
          ["Stock quantity cannot be negative."];
    }

    if (command.CategoryId == Guid.Empty)
    {
      errors["categoryId"] = ["Category is required."];
    }

    if (command.BrandId == Guid.Empty)
    {
      errors["brandId"] = ["Brand is required."];
    }

    if (errors.Count > 0)
    {
      throw new ValidationException(errors);
    }
  }
}
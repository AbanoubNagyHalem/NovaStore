namespace NovaStore.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    string Name,
    string Slug,
    string? Description,
    string Sku,
    decimal Price,
    decimal? CompareAtPrice,
    int StockQuantity,
    bool IsFeatured,
    Guid CategoryId,
    Guid BrandId);
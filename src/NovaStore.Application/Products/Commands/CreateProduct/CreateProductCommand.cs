namespace NovaStore.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
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
namespace NovaStore.Application.Products.Common;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string Sku,
    decimal Price,
    decimal? CompareAtPrice,
    int StockQuantity,
    bool IsFeatured,
    Guid CategoryId,
    string CategoryName,
    Guid BrandId,
    string BrandName);
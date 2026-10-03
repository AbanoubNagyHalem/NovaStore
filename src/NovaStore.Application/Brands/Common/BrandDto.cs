namespace NovaStore.Application.Brands.Common;

public sealed record BrandDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? LogoUrl,
    bool IsActive);
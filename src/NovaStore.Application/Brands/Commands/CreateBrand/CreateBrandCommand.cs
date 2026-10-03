namespace NovaStore.Application.Brands.Commands.CreateBrand;

public sealed record CreateBrandCommand(
    string Name,
    string Slug,
    string? Description,
    string? LogoUrl);
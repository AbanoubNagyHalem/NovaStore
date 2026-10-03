namespace NovaStore.Application.Categories.Common;

public sealed record CategoryDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive);
using NovaStore.Application.Brands.Common;
using NovaStore.Application.Common.Exceptions;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Domain.Entities;

namespace NovaStore.Application.Brands.Commands.CreateBrand;

public sealed class CreateBrandCommandHandler
{
  private readonly IBrandRepository _brandRepository;

  public CreateBrandCommandHandler(
      IBrandRepository brandRepository)
  {
    _brandRepository = brandRepository;
  }

  public async Task<BrandDto> HandleAsync(
      CreateBrandCommand command,
      CancellationToken cancellationToken = default)
  {
    Validate(command);

    var name = command.Name.Trim();
    var slug = command.Slug.Trim().ToLowerInvariant();

    var slugExists = await _brandRepository.ExistsBySlugAsync(
        slug,
        cancellationToken);

    if (slugExists)
    {
      throw new ConflictException(
          $"A brand with the slug '{slug}' already exists.");
    }

    var brand = new Brand
    {
      Id = Guid.NewGuid(),
      Name = name,
      Slug = slug,
      Description = string.IsNullOrWhiteSpace(command.Description)
            ? null
            : command.Description.Trim(),
      LogoUrl = string.IsNullOrWhiteSpace(command.LogoUrl)
            ? null
            : command.LogoUrl.Trim(),
      IsActive = true,
      CreatedAtUtc = DateTime.UtcNow
    };

    await _brandRepository.AddAsync(
        brand,
        cancellationToken);

    await _brandRepository.SaveChangesAsync(
        cancellationToken);

    return new BrandDto(
        brand.Id,
        brand.Name,
        brand.Slug,
        brand.Description,
        brand.LogoUrl,
        brand.IsActive);
  }

  private static void Validate(
      CreateBrandCommand command)
  {
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(command.Name))
    {
      errors["name"] =
      [
          "Brand name is required."
      ];
    }
    else if (command.Name.Trim().Length > 100)
    {
      errors["name"] =
      [
          "Brand name must not exceed 100 characters."
      ];
    }

    if (string.IsNullOrWhiteSpace(command.Slug))
    {
      errors["slug"] =
      [
          "Brand slug is required."
      ];
    }
    else if (command.Slug.Trim().Length > 120)
    {
      errors["slug"] =
      [
          "Brand slug must not exceed 120 characters."
      ];
    }

    if (command.Description?.Trim().Length > 1000)
    {
      errors["description"] =
      [
          "Brand description must not exceed 1000 characters."
      ];
    }

    if (command.LogoUrl?.Trim().Length > 500)
    {
      errors["logoUrl"] =
      [
          "Brand logo URL must not exceed 500 characters."
      ];
    }

    if (errors.Count > 0)
    {
      throw new ValidationException(errors);
    }
  }
}
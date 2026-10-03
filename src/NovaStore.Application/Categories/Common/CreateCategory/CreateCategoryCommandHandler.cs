using NovaStore.Application.Categories.Common;
using NovaStore.Application.Common.Exceptions;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Domain.Entities;

namespace NovaStore.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler
{
  private readonly ICategoryRepository _categoryRepository;

  public CreateCategoryCommandHandler(
      ICategoryRepository categoryRepository)
  {
    _categoryRepository = categoryRepository;
  }

  public async Task<CategoryDto> HandleAsync(
      CreateCategoryCommand command,
      CancellationToken cancellationToken = default)
  {
    Validate(command);

    var name = command.Name.Trim();
    var slug = command.Slug.Trim().ToLowerInvariant();

    var slugExists = await _categoryRepository.ExistsBySlugAsync(
        slug,
        cancellationToken);

    if (slugExists)
    {
      throw new ConflictException(
          $"A category with the slug '{slug}' already exists.");
    }

    var category = new Category
    {
      Id = Guid.NewGuid(),
      Name = name,
      Slug = slug,
      Description = string.IsNullOrWhiteSpace(command.Description)
            ? null
            : command.Description.Trim(),
      IsActive = true,
      CreatedAtUtc = DateTime.UtcNow
    };

    await _categoryRepository.AddAsync(
        category,
        cancellationToken);

    await _categoryRepository.SaveChangesAsync(
        cancellationToken);

    return new CategoryDto(
        category.Id,
        category.Name,
        category.Slug,
        category.Description,
        category.IsActive);
  }

  private static void Validate(
      CreateCategoryCommand command)
  {
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(command.Name))
    {
      errors["name"] =
      [
          "Category name is required."
      ];
    }
    else if (command.Name.Trim().Length > 100)
    {
      errors["name"] =
      [
          "Category name must not exceed 100 characters."
      ];
    }

    if (string.IsNullOrWhiteSpace(command.Slug))
    {
      errors["slug"] =
      [
          "Category slug is required."
      ];
    }
    else if (command.Slug.Trim().Length > 120)
    {
      errors["slug"] =
      [
          "Category slug must not exceed 120 characters."
      ];
    }

    if (command.Description?.Trim().Length > 1000)
    {
      errors["description"] =
      [
          "Category description must not exceed 1000 characters."
      ];
    }

    if (errors.Count > 0)
    {
      throw new ValidationException(errors);
    }
  }
}
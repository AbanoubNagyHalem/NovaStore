using NovaStore.Application.Categories.Commands.CreateCategory;

namespace NovaStore.Api.Endpoints;

public static class CategoriesEndpoints
{
  public static IEndpointRouteBuilder MapCategoriesEndpoints(
      this IEndpointRouteBuilder endpoints)
  {
    var group = endpoints.MapGroup("/api/categories")
        .WithTags("Categories");

    group.MapPost("/", CreateCategoryAsync);

    return endpoints;
  }

  private static async Task<IResult> CreateCategoryAsync(
      CreateCategoryCommand command,
      CreateCategoryCommandHandler handler,
      CancellationToken cancellationToken)
  {
    var category = await handler.HandleAsync(
        command,
        cancellationToken);

    return Results.Created(
        $"/api/categories/{category.Id}",
        category);
  }
}
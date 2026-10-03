using NovaStore.Application.Brands.Commands.CreateBrand;

namespace NovaStore.Api.Endpoints;

public static class BrandsEndpoints
{
  public static IEndpointRouteBuilder MapBrandsEndpoints(
      this IEndpointRouteBuilder endpoints)
  {
    var group = endpoints.MapGroup("/api/brands")
        .WithTags("Brands");

    group.MapPost("/", CreateBrandAsync);

    return endpoints;
  }

  private static async Task<IResult> CreateBrandAsync(
      CreateBrandCommand command,
      CreateBrandCommandHandler handler,
      CancellationToken cancellationToken)
  {
    var brand = await handler.HandleAsync(
        command,
        cancellationToken);

    return Results.Created(
        $"/api/brands/{brand.Id}",
        brand);
  }
}
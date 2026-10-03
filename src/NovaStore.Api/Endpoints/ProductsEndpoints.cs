using NovaStore.Application.Products.Queries.GetProducts;

namespace NovaStore.Api.Endpoints;

public static class ProductsEndpoints
{
  public static IEndpointRouteBuilder MapProductsEndpoints(
      this IEndpointRouteBuilder endpoints)
  {
    var group = endpoints.MapGroup("/api/products")
        .WithTags("Products");

    group.MapGet("/", GetProductsAsync);

    return endpoints;
  }

  private static async Task<IResult> GetProductsAsync(
      GetProductsQuery query,
      CancellationToken cancellationToken)
  {
    var products = await query.ExecuteAsync(cancellationToken);

    return Results.Ok(products);
  }
}
using NovaStore.Application.Products.Commands.CreateProduct;
using NovaStore.Application.Products.Commands.UpdateProduct;
using NovaStore.Application.Products.Queries.GetProductById;
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
    group.MapGet("/{id:guid}", GetProductByIdAsync);
    group.MapPost("/", CreateProductAsync);
    group.MapPut("/{id:guid}", UpdateProductAsync);

    return endpoints;
  }

  private static async Task<IResult> GetProductsAsync(
      GetProductsQuery query,
      CancellationToken cancellationToken)
  {
    var products = await query.ExecuteAsync(cancellationToken);

    return Results.Ok(products);
  }

  private static async Task<IResult> GetProductByIdAsync(
      Guid id,
      GetProductByIdQuery query,
      CancellationToken cancellationToken)
  {
    var product = await query.ExecuteAsync(
        id,
        cancellationToken);

    return Results.Ok(product);
  }

  private static async Task<IResult> CreateProductAsync(
      CreateProductCommand command,
      CreateProductCommandHandler handler,
      CancellationToken cancellationToken)
  {
    var product = await handler.HandleAsync(
        command,
        cancellationToken);

    return Results.Created(
        $"/api/products/{product.Id}",
        product);
  }

  private static async Task<IResult> UpdateProductAsync(
      Guid id,
      UpdateProductCommand command,
      UpdateProductCommandHandler handler,
      CancellationToken cancellationToken)
  {
    var product = await handler.HandleAsync(
        id,
        command,
        cancellationToken);

    return Results.Ok(product);
  }
}
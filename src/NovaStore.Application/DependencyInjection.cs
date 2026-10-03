using Microsoft.Extensions.DependencyInjection;
using NovaStore.Application.Brands.Commands.CreateBrand;
using NovaStore.Application.Categories.Commands.CreateCategory;
using NovaStore.Application.Products.Commands.CreateProduct;
using NovaStore.Application.Products.Queries.GetProductById;
using NovaStore.Application.Products.Queries.GetProducts;

namespace NovaStore.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(
      this IServiceCollection services)
  {
    services.AddScoped<GetProductsQuery>();
    services.AddScoped<GetProductByIdQuery>();
    services.AddScoped<CreateCategoryCommandHandler>();
    services.AddScoped<CreateBrandCommandHandler>();
    services.AddScoped<CreateProductCommandHandler>();

    return services;
  }
}
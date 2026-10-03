using Microsoft.Extensions.DependencyInjection;
using NovaStore.Application.Categories.Commands.CreateCategory;
using NovaStore.Application.Products.Queries.GetProducts;

namespace NovaStore.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(
      this IServiceCollection services)
  {
    services.AddScoped<GetProductsQuery>();
    services.AddScoped<CreateCategoryCommandHandler>();

    return services;
  }
}
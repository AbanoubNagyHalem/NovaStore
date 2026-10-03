using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovaStore.Application.Common.Interfaces;
using NovaStore.Infrastructure.Persistence;
using NovaStore.Infrastructure.Repositories;

namespace NovaStore.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' was not found.");

    services.AddDbContext<NovaStoreDbContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();

    return services;
  }
}
namespace NovaStore.Application.Products.Queries.GetProducts;

public sealed record ProductQueryParameters(
    int PageNumber = 1,
    int PageSize = 20);
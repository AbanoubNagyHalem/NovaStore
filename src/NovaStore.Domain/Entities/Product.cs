namespace NovaStore.Domain.Entities;

public class Product
{
  public Guid Id { get; set; }

  public string Name { get; set; } = string.Empty;

  public string Slug { get; set; } = string.Empty;

  public string? Description { get; set; }

  public string Sku { get; set; } = string.Empty;

  public decimal Price { get; set; }

  public decimal? CompareAtPrice { get; set; }

  public int StockQuantity { get; set; }

  public bool IsActive { get; set; } = true;

  public bool IsFeatured { get; set; }

  public DateTime CreatedAtUtc { get; set; }

  public DateTime? UpdatedAtUtc { get; set; }

  public Guid CategoryId { get; set; }

  public Category Category { get; set; } = null!;

  public Guid BrandId { get; set; }

  public Brand Brand { get; set; } = null!;
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaStore.Domain.Entities;

namespace NovaStore.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.ToTable("Products");

    builder.HasKey(product => product.Id);

    builder.Property(product => product.Name)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(product => product.Slug)
        .IsRequired()
        .HasMaxLength(220);

    builder.HasIndex(product => product.Slug)
        .IsUnique();

    builder.Property(product => product.Description)
        .HasMaxLength(4000);

    builder.Property(product => product.Sku)
        .IsRequired()
        .HasMaxLength(100);

    builder.HasIndex(product => product.Sku)
        .IsUnique();

    builder.Property(product => product.Price)
        .HasPrecision(18, 2);

    builder.Property(product => product.CompareAtPrice)
        .HasPrecision(18, 2);

    builder.HasOne(product => product.Category)
        .WithMany(category => category.Products)
        .HasForeignKey(product => product.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.HasOne(product => product.Brand)
        .WithMany(brand => brand.Products)
        .HasForeignKey(product => product.BrandId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
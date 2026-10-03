using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaStore.Domain.Entities;

namespace NovaStore.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
  public void Configure(EntityTypeBuilder<Category> builder)
  {
    builder.ToTable("Categories");

    builder.HasKey(category => category.Id);

    builder.Property(category => category.Name)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(category => category.Slug)
        .IsRequired()
        .HasMaxLength(120);

    builder.HasIndex(category => category.Slug)
        .IsUnique();

    builder.Property(category => category.Description)
        .HasMaxLength(1000);
  }
}
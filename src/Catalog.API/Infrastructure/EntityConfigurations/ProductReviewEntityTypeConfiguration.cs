using eShop.Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Catalog.API.Infrastructure.EntityConfigurations;

public class ProductReviewEntityTypeConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.ToTable("ProductReviews");

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .UseHiLo("product_review_hilo")
            .IsRequired();
            
        builder.Property(r => r.UserId)
            .IsRequired();
            
        builder.Property(r => r.Rating)
            .IsRequired();
            
        builder.Property(r => r.ReviewText)
            .HasMaxLength(1000);
            
        builder.Property(r => r.CreatedAt)
            .IsRequired();
            
        builder.HasOne(r => r.CatalogItem)
            .WithMany(c => c.Reviews)
            .HasForeignKey(r => r.CatalogItemId);
    }
}
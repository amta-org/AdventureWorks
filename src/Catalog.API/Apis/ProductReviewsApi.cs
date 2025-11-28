using eShop.Catalog.API.Infrastructure;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace eShop.Catalog.API.Apis;

public static class ProductReviewsApi
{
    public static IEndpointRouteBuilder MapProductReviewsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog/items");
        
        // Get all reviews for a product
        api.MapGet("/{itemId:int}/reviews", GetProductReviews)
            .WithName("GetProductReviews")
            .WithSummary("Get reviews for a catalog item")
            .WithDescription("Get all reviews for a specific catalog item")
            .WithTags("Reviews");
            
        // Submit a review for a product
        api.MapPost("/{itemId:int}/reviews", SubmitProductReview)
            .WithName("SubmitProductReview")
            .WithSummary("Submit a review for a catalog item")
            .WithDescription("Submit a rating and review for a catalog item (only for purchased products)")
            .WithTags("Reviews");
            
        return app;
    }
    
    public static async Task<Results<Ok<ProductReviewSummary>, NotFound>> GetProductReviews(
        int itemId,
        CatalogContext context)
    {
        // Check if the product exists
        var product = await context.CatalogItems
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == itemId);
            
        if (product == null)
        {
            return TypedResults.NotFound();
        }
        
        // Calculate average rating
        double averageRating = product.Reviews?.Any() == true 
            ? product.Reviews.Average(r => r.Rating) 
            : 0;
            
        // Map reviews to the response DTO
        var reviewResponses = product.Reviews?.Select(r => new ProductReviewResponse
        {
            Id = r.Id,
            UserId = r.UserId,
            Rating = r.Rating,
            ReviewText = r.ReviewText,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        }).ToList() ?? new List<ProductReviewResponse>();
        
        var summary = new ProductReviewSummary
        {
            AverageRating = averageRating,
            TotalReviews = reviewResponses.Count,
            Reviews = reviewResponses
        };
        
        return TypedResults.Ok(summary);
    }
    
    public static async Task<Results<Created<ProductReviewResponse>, NotFound, BadRequest<string>>> SubmitProductReview(
        int itemId,
        ProductReviewDto reviewDto,
        CatalogContext context,
        IPurchaseValidationService purchaseValidationService,
        IHttpContextAccessor httpContextAccessor)
    {
        // Get the authenticated user ID
        var userId = httpContextAccessor?.HttpContext?.User?.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return TypedResults.BadRequest("User must be authenticated to submit a review");
        }
        
        // Check if the product exists
        var product = await context.CatalogItems
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == itemId);
            
        if (product == null)
        {
            return TypedResults.NotFound();
        }
        
        // Check if the user has already submitted a review for this product
        var existingReview = await context.ProductReviews
            .FirstOrDefaultAsync(r => r.UserId == userId && r.CatalogItemId == itemId);
            
        if (existingReview != null)
        {
            // Update existing review
            existingReview.Rating = reviewDto.Rating;
            existingReview.ReviewText = reviewDto.ReviewText;
            existingReview.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            // Check if the user has purchased the product
            bool hasPurchased = await purchaseValidationService.HasUserPurchasedProductAsync(userId, itemId);
            
            if (!hasPurchased)
            {
                return TypedResults.BadRequest("You can only review products you have purchased");
            }
            
            // Create new review
            existingReview = new ProductReview
            {
                UserId = userId,
                CatalogItemId = itemId,
                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText,
                CreatedAt = DateTime.UtcNow
            };
            
            context.ProductReviews.Add(existingReview);
        }
        
        await context.SaveChangesAsync();
        
        // Update product average rating
        product.AverageRating = product.Reviews.Average(r => r.Rating);
        await context.SaveChangesAsync();
        
        // Map to response
        var response = new ProductReviewResponse
        {
            Id = existingReview.Id,
            UserId = existingReview.UserId,
            Rating = existingReview.Rating,
            ReviewText = existingReview.ReviewText,
            CreatedAt = existingReview.CreatedAt,
            UpdatedAt = existingReview.UpdatedAt
        };
        
        return TypedResults.Created($"/api/catalog/items/{itemId}/reviews/{existingReview.Id}", response);
    }
}
using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.API.Model;

public record ProductReviewDto
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; init; }

    [MaxLength(1000)]
    public string ReviewText { get; init; }
}

public record ProductReviewResponse
{
    public int Id { get; init; }
    public string UserId { get; init; }
    public int Rating { get; init; }
    public string ReviewText { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record ProductReviewSummary
{
    public double AverageRating { get; init; }
    public int TotalReviews { get; init; }
    public IEnumerable<ProductReviewResponse> Reviews { get; init; }
}
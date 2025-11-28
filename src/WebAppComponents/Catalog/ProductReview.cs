namespace eShop.WebAppComponents.Catalog;

public record ProductReviewResponse(
    int Id,
    string UserId,
    int Rating,
    string ReviewText,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

public record ProductReviewSummary(
    double AverageRating,
    int TotalReviews,
    IEnumerable<ProductReviewResponse> Reviews);

public record ProductReviewRequest(
    int Rating,
    string ReviewText);
namespace eShop.WebAppComponents.Item;

public record ProductReview(
    int Id,
    int ProductId,
    string UserId,
    string UserName,
    int StarRating,
    string ReviewText,
    DateTime CreatedDate);

public record ProductRatingsSummary(
    double AverageRating,
    int TotalReviews,
    int[] RatingCounts); // Index 0 = 1-star count, Index 1 = 2-star count, etc.
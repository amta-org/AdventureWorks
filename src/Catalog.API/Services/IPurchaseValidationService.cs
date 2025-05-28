namespace eShop.Catalog.API.Services;

public interface IPurchaseValidationService
{
    Task<bool> HasUserPurchasedProductAsync(string userId, int productId);
}
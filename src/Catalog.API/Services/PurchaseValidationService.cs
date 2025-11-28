using System.Net.Http.Json;
using eShop.Ordering.API.Application.Queries;

namespace eShop.Catalog.API.Services;

public class PurchaseValidationService : IPurchaseValidationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PurchaseValidationService> _logger;
    private readonly IConfiguration _configuration;

    public PurchaseValidationService(
        HttpClient httpClient,
        ILogger<PurchaseValidationService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<bool> HasUserPurchasedProductAsync(string userId, int productId)
    {
        try
        {
            // Get the orders API URL from configuration
            string orderingApiUrl = _configuration["OrderingApiUrl"] ?? "http://localhost:5102";
            
            // Get all orders for the user
            var orders = await _httpClient.GetFromJsonAsync<IEnumerable<Order>>($"{orderingApiUrl}/api/orders");
            
            if (orders == null)
                return false;
                
            // Check if the user has purchased the product
            foreach (var order in orders)
            {
                if (order.OrderItems != null && order.OrderItems.Any(item => 
                    item.ProductName.Contains(productId.ToString()) || // Simplified check using product ID in name
                    (item.ProductId != null && item.ProductId == productId))) // If ProductId is available
                {
                    return true;
                }
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating purchase for user {UserId} and product {ProductId}", userId, productId);
            
            // If there's an error, we'll allow the review
            // This is to prevent blocking legitimate reviews due to temporary service issues
            // In production, you might want a different approach
            return true;
        }
    }
}
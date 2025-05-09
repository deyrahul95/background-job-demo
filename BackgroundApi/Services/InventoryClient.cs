using BackgroundApi.Models;
using BackgroundApi.Services.Interfaces;

namespace BackgroundApi.Services;

public class InventoryClient(
    HttpClient httpClient,
    ILogger<InventoryClient> logger) : IInventoryClient
{
    public async Task<GetInventoryResponse?> GetInventory(int productId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<GetInventoryResponse>($"/api/inventory/{productId}");   
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get inventory");
            return null;
        }
    }
}

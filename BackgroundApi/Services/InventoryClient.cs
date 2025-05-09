using System.Net.Http.Headers;
using BackgroundApi.Models;
using BackgroundApi.Services.Interfaces;

namespace BackgroundApi.Services;

public class InventoryClient(
    HttpClient httpClient,
    ILogger<InventoryClient> logger) : IInventoryClient
{
    public async Task<GetInventoryResponse?> GetInventory(int productId, string? token = null)
    {
        try
        {
            if (string.IsNullOrEmpty(token) is false)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); ;
            }

            return await httpClient.GetFromJsonAsync<GetInventoryResponse>($"/api/inventory/{productId}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get inventory");
            return null;
        }
    }
}

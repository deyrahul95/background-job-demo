using BackgroundApi.Models;

namespace BackgroundApi.Services.Interfaces;

public interface IInventoryClient
{
    Task<GetInventoryResponse?> GetInventory(int productId);
}

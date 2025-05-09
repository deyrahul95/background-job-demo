namespace BackgroundApi.Models;

public record InventoryJob(int OrderId, List<int> ProductIds, int Quantity);

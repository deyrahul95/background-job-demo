namespace MessageBrokerApi.Models;

public sealed record CreateOrderRequest(int ProductId, int Quantity, string PaymentMode);

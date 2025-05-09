using BackgroundApi.Enums;

namespace BackgroundApi.Services.Interfaces;

public interface IOrderService
{
    Task<JobStatus> CreateOrder(int itemCount);
    Task<JobStatus> GetOrder(int id);
}

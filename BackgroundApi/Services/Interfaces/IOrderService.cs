using BackgroundApi.Enums;
using BackgroundApi.Models;

namespace BackgroundApi.Services.Interfaces;

public interface IOrderService
{
    Task<JobStatus> CreateOrder(int itemCount);
    Task<JobStatusDto?> GetOrder(int id);
}

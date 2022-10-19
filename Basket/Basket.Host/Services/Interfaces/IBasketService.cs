using Basket.Host.Models;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task<GetBasketResponse> GetAsync(string userId);

    Task UpdateAsync(string userId, string data);

    Task DeleteAsync(string userId);
}

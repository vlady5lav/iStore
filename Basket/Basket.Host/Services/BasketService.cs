using Basket.Host.Models;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ICacheService _cacheService;

    public BasketService(
        ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<GetBasketResponse> GetAsync(string userId)
    {
        var result = await _cacheService.GetAsync<string>(userId);
        return new GetBasketResponse() { Data = result ?? "{}" };
    }

    public async Task UpdateAsync(string userId, string data)
    {
        await _cacheService.AddOrUpdateAsync(userId, data);
    }

    public async Task DeleteAsync(string userId)
    {
        await _cacheService.DeleteAsync(userId);
    }
}

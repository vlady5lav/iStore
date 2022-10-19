using Basket.Host.Models;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Scope("basket.bff")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly IBasketService _basketService;

    private readonly ILogger<BasketBffController> _logger;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetBasketResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.TooManyRequests)]
    public async Task<IActionResult> GetBasket()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        var response = await _basketService.GetAsync(userId!);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.TooManyRequests)]
    public async Task<IActionResult> UpdateBasket(UpdateBasketRequest data)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        await _basketService.UpdateAsync(userId!, data.Data);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.TooManyRequests)]
    public async Task<IActionResult> DeleteBasket()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

        await _basketService.DeleteAsync(userId!);

        return Ok();
    }
}

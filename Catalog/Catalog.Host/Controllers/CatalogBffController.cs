using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Scope("catalog.bff")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogBffController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    private readonly ILogger<CatalogBffController> _logger;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogService catalogService)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CatalogBrandDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBrands()
    {
        var result = await _catalogService.GetBrandsAsync();

        if (result != null && result.Any())
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedItemsResponse<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetItems(PaginatedItemsRequest request)
    {
        var result =
            await _catalogService.GetItemsByPageAsync(
                request.PageSize,
                request.PageIndex,
                request.BrandIdFilter,
                request.TypeIdFilter);

        if (result != null && result.Data.Any())
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CatalogItemDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetItem(int id)
    {
        var result = await _catalogService.GetItemByIdAsync(id);

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CatalogItemDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetAllItems()
    {
        var result = await _catalogService.GetAllItemsAsync();

        if (result != null && result.Any())
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<CatalogTypeDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetTypes()
    {
        var result = await _catalogService.GetTypesAsync();

        if (result != null && result.Any())
        {
            return Ok(result);
        }
        else
        {
            return NotFound();
        }
    }
}

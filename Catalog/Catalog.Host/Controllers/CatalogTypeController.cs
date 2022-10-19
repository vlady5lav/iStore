using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("catalog")]
[Route(ComponentDefaults.DefaultRoute)]
public class CatalogTypeController : ControllerBase
{
    private readonly ICatalogTypeService _catalogTypeService;

    private readonly ILogger<CatalogTypeController> _logger;

    public CatalogTypeController(
        ILogger<CatalogTypeController> logger,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTypeResponse<int>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Add(CreateTypeRequest request)
    {
        var result = await _catalogTypeService.AddAsync(request.Name);

        if (result != null)
        {
            return Ok(new CreateTypeResponse<int?>() { Id = result });
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _catalogTypeService.DeleteAsync(id);

        if (result != null)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UpdateTypeRequest request)
    {
        var result = await _catalogTypeService.UpdateAsync(request.Id, request.Name);

        if (result != null)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }
}

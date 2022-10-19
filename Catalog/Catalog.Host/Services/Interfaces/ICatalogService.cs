using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<IEnumerable<CatalogBrandDto>?> GetBrandsAsync();

    Task<PaginatedItemsResponse<CatalogItemDto>?> GetItemsByPageAsync(
        int pageSize = 10,
        int pageIndex = 0,
        int[]? brandIdFilter = null,
        int[]? typeIdFilter = null);

    Task<CatalogItemDto?> GetItemByIdAsync(int id);

    Task<IEnumerable<CatalogItemDto>?> GetAllItemsAsync();

    Task<IEnumerable<CatalogTypeDto>?> GetTypesAsync();
}

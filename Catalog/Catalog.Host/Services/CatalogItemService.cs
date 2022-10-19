using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task<int?> AddAsync(
        string name,
        int availableStock,
        decimal price,
        int warranty,
        int catalogBrandId,
        int catalogTypeId,
        string? description = null,
        string? pictureFileName = null)
    {
        return await ExecuteSafeAsync(
            async () =>
                await _catalogItemRepository.AddAsync(
                    name,
                    availableStock,
                    price,
                    warranty,
                    catalogBrandId,
                    catalogTypeId,
                    description,
                    pictureFileName));
    }

    public async Task<int?> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () => await _catalogItemRepository.DeleteAsync(id));
    }

    public async Task<int?> UpdateAsync(
        int id,
        string? name = null,
        int? availableStock = null,
        decimal? price = null,
        int? warranty = null,
        int? catalogBrandId = null,
        int? catalogTypeId = null,
        string? description = null,
        string? pictureFileName = null)
    {
        return await ExecuteSafeAsync(
            async () =>
                await _catalogItemRepository.UpdateAsync(
                    id,
                    name,
                    availableStock,
                    price,
                    warranty,
                    catalogBrandId,
                    catalogTypeId,
                    description,
                    pictureFileName));
    }
}

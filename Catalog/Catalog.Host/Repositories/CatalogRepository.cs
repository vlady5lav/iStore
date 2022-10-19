using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<CatalogBrand?>?> GetBrandsAsync()
    {
        var result = await _dbContext.CatalogBrands.OrderBy(cb => cb.Name).ToListAsync();

        return result;
    }

    public async Task<CatalogItem?> GetByIdAsync(int id)
    {
        var result = await _dbContext.CatalogItems
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .FirstOrDefaultAsync(ci => ci.Id == id);

        return result;
    }

    public async Task<PaginatedItems<CatalogItem?>?> GetByPageAsync(
        int pageSize,
        int pageIndex,
        int[]? brandFilter = null,
        int[]? typeFilter = null)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (brandFilter?.Length > 0)
        {
            query = query.Where(w => brandFilter.Contains(w.CatalogBrandId));
        }

        if (typeFilter?.Length > 0)
        {
            query = query.Where(w => typeFilter.Contains(w.CatalogTypeId));
        }

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query
            .OrderBy(ci => ci.CatalogBrandId)
            .ThenBy(ci => ci.Name)
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem?>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<IEnumerable<CatalogItem?>?> GetAllItemsAsync()
    {
        var result = await _dbContext.CatalogItems
            .OrderBy(ci => ci.CatalogBrandId)
            .ThenBy(ci => ci.Name)
            .Include(ci => ci.CatalogBrand)
            .Include(ci => ci.CatalogType)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<CatalogType?>?> GetTypesAsync()
    {
        var result = await _dbContext.CatalogTypes.OrderBy(ct => ct.Name).ToListAsync();

        return result;
    }
}

using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogBrandRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddAsync(string name)
    {
        var id = await _dbContext.CatalogBrands.CountAsync() + 1; // entities ids starts with 1

        var addItem = new CatalogBrand { Id = id, Name = name, };

        var item = await _dbContext.CatalogBrands.AddAsync(addItem);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var item = await _dbContext.CatalogBrands.FirstOrDefaultAsync(cb => cb.Id == id);

        if (item != null)
        {
            _dbContext.CatalogBrands.Remove(item);

            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
        else
        {
            return null;
        }
    }

    public async Task<int?> UpdateAsync(int id, string name)
    {
        var item = await _dbContext.CatalogBrands.FirstOrDefaultAsync(cb => cb.Id == id);

        if (item != null)
        {
            item.Name = name;

            _dbContext.CatalogBrands.Update(item);

            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
        else
        {
            return null;
        }
    }
}

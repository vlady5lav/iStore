using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogTypeRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddAsync(string name)
    {
        var id = await _dbContext.CatalogTypes.CountAsync() + 1; // entities ids starts with 1

        var addItem = new CatalogType { Id = id, Name = name, };

        var item = await _dbContext.CatalogTypes.AddAsync(addItem);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var item = await _dbContext.CatalogTypes.FirstOrDefaultAsync(ct => ct.Id == id);

        if (item != null)
        {
            _dbContext.CatalogTypes.Remove(item);

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
        var item = await _dbContext.CatalogTypes.FirstOrDefaultAsync(ct => ct.Id == id);

        if (item != null)
        {
            item.Name = name;

            _dbContext.CatalogTypes.Update(item);

            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
        else
        {
            return null;
        }
    }
}

using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
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
        var id = await _dbContext.CatalogItems.CountAsync() + 1; // entity ids starts with 1

        var addItem = new CatalogItem
        {
            Id = id,
            Name = name,
            AvailableStock = availableStock,
            Price = price,
            Warranty = warranty,
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description ?? null,
            PictureFileName = pictureFileName ?? null,
        };

        var item = await _dbContext.CatalogItems.AddAsync(addItem);

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            _dbContext.CatalogItems.Remove(item);

            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
        else
        {
            return null;
        }
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
        var item = await _dbContext.CatalogItems.FirstOrDefaultAsync(ci => ci.Id == id);

        if (item != null)
        {
            item.Name = name ?? item.Name;
            item.AvailableStock = availableStock ?? item.AvailableStock;
            item.Price = price ?? item.Price;
            item.Warranty = warranty ?? item.Warranty;
            item.CatalogBrandId = catalogBrandId ?? item.CatalogBrandId;
            item.CatalogTypeId = catalogTypeId ?? item.CatalogTypeId;
            item.Description = description ?? item.Description;
            item.PictureFileName = pictureFileName ?? item.PictureFileName;

            _dbContext.CatalogItems.Update(item);

            await _dbContext.SaveChangesAsync();

            return item.Id;
        }
        else
        {
            return null;
        }
    }
}

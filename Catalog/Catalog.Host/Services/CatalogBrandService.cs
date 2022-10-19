using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;

    public CatalogBrandService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogBrandRepository catalogBrandRepository)
        : base(dbContextWrapper, logger)
    {
        _catalogBrandRepository = catalogBrandRepository;
    }

    public async Task<int?> AddAsync(string name)
    {
        return await ExecuteSafeAsync(async () => await _catalogBrandRepository.AddAsync(name));
    }

    public async Task<int?> DeleteAsync(int id)
    {
        return await ExecuteSafeAsync(async () => await _catalogBrandRepository.DeleteAsync(id));
    }

    public async Task<int?> UpdateAsync(int id, string name)
    {
        return await ExecuteSafeAsync(async () => await _catalogBrandRepository.UpdateAsync(id, name));
    }
}

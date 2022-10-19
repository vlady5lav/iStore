namespace Catalog.Host.Services.Interfaces;

public interface ICatalogBrandService
{
    Task<int?> AddAsync(string name);

    Task<int?> DeleteAsync(int id);

    Task<int?> UpdateAsync(int id, string name);
}

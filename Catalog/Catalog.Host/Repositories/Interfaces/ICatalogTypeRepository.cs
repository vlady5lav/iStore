namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogTypeRepository
{
    Task<int?> AddAsync(string name);

    Task<int?> DeleteAsync(int id);

    Task<int?> UpdateAsync(int id, string name);
}

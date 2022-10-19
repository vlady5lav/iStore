namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<int?> AddAsync(
    string name,
    int availableStock,
    decimal price,
    int warranty,
    int catalogBrandId,
    int catalogTypeId,
    string? description = null,
    string? pictureFileName = null);

    Task<int?> DeleteAsync(
        int id);

    Task<int?> UpdateAsync(
        int id,
        string? name = null,
        int? availableStock = null,
        decimal? price = null,
        int? warranty = null,
        int? catalogBrandId = null,
        int? catalogTypeId = null,
        string? description = null,
        string? pictureFileName = null);
}

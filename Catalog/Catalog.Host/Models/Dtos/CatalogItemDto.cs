namespace Catalog.Host.Models.Dtos;

public class CatalogItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int AvailableStock { get; set; }

    public decimal Price { get; set; }

    public int Warranty { get; set; }

    public string? Description { get; set; }

    public string? PictureUrl { get; set; }

    public CatalogBrandDto CatalogBrand { get; set; } = null!;

    public CatalogTypeDto CatalogType { get; set; } = null!;
}

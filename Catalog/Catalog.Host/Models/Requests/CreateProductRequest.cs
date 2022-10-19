namespace Catalog.Host.Models.Requests;

public class CreateProductRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Product Name")]
    public string Name { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Available Stock value")]
    [Range(0, int.MaxValue, ErrorMessage = "You should correctly specify the Available Stock value")]
    public int AvailableStock { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Price value")]
    [Range(0, double.MaxValue, ErrorMessage = "You should correctly specify the Price value")]
    public decimal Price { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Available Stock value")]
    [Range(0, int.MaxValue, ErrorMessage = "You should correctly specify the Warranty value")]
    public int Warranty { get; set; }

    public string? Description { get; set; } = null;

    public string? PictureFileName { get; set; } = null;

    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Catalog Brand Id")]
    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify the Catalog Brand Id")]
    public int CatalogBrandId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Catalog Type Id")]
    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify the Catalog Type Id")]
    public int CatalogTypeId { get; set; }
}

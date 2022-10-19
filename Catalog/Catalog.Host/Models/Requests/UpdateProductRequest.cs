namespace Catalog.Host.Models.Requests;

public class UpdateProductRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the ID")]
    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify the ID")]
    public int Id { get; set; }

    public string? Name { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "You should correctly specify the Available Stock value")]
    public int? AvailableStock { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "You should correctly specify the Price value")]
    public decimal? Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "You should correctly specify the Warranty value")]
    public int? Warranty { get; set; }

    public string? Description { get; set; }

    public string? PictureFileName { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify the Catalog Brand Id")]
    public int? CatalogBrandId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify the Catalog Type Id")]
    public int? CatalogTypeId { get; set; }
}

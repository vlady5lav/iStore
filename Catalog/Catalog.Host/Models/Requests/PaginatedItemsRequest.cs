namespace Catalog.Host.Models.Requests;

public class PaginatedItemsRequest
{
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "You should correctly specify PageIndex!")]
    public int PageIndex { get; set; } = 0;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify PageSize!")]
    public int PageSize { get; set; } = 10;

    public int[]? BrandIdFilter { get; set; } = null;

    public int[]? TypeIdFilter { get; set; } = null;
}

namespace Catalog.Host.Models.Requests;

public class UpdateBrandRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the ID")]
    [Range(1, int.MaxValue, ErrorMessage = "You should correctly specify the ID")]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Brand Name")]
    public string Name { get; set; } = null!;
}

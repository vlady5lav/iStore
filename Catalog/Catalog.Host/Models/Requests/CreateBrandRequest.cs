namespace Catalog.Host.Models.Requests;

public class CreateBrandRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Brand Name")]
    public string Name { get; set; } = null!;
}

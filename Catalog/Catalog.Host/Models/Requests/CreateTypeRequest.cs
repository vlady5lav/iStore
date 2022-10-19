namespace Catalog.Host.Models.Requests;

public class CreateTypeRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "You should specify the Type Name")]
    public string Name { get; set; } = null!;
}

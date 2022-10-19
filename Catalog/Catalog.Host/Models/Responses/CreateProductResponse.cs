namespace Catalog.Host.Models.Responses;

public class CreateProductResponse<T>
{
    public T Id { get; set; } = default!;
}

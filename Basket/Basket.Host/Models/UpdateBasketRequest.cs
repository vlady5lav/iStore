namespace Basket.Host.Models;

public class UpdateBasketRequest
{
    [Required]
    public string Data { get; set; } = null!;
}

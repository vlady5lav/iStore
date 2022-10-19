namespace Catalog.Host.Configurations;

public class CatalogConfig
{
    public const string Catalog = "Catalog";

    public string CdnHost { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;
}

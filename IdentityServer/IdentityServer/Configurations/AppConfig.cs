namespace IdentityServer.Configurations;

public class AppConfig
{
    public const string App = "App";

    public string BasePath { get; set; } = null!;

    public string BasketApi { get; set; } = null!;

    public string CatalogApi { get; set; } = null!;

    public string GlobalUrl { get; set; } = null!;

    public string IdentityUrl { get; set; } = null!;

    public string SpaUrl { get; set; } = null!;

    public string HttpLogging { get; set; } = null!;
}

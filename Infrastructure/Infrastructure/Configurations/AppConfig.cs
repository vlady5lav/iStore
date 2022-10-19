namespace Infrastructure.Configurations;

public class AppConfig
{
    public const string App = "App";

    public string BasePath { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public string GlobalUrl { get; set; } = null!;

    public string SpaUrl { get; set; } = null!;

    public string HttpLogging { get; set; } = null!;
}

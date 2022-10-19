namespace Catalog.Host.Configurations;

public class DatabaseConfig
{
    public const string Database = "Database";

    public string ConnectionString { get; set; } = null!;

    public string EnvVar { get; set; } = null!;
}

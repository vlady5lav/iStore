using Catalog.Host.Configurations;
using Catalog.Host.Data.Entities;
using Catalog.Host.Data.EntityConfigurations;

namespace Catalog.Host.Data;

public class ApplicationDbContext : DbContext
{
    private readonly DatabaseConfig _config;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptionsMonitor<DatabaseConfig> config)
        : base(options)
    {
        _config = config.CurrentValue;
    }

    public DbSet<CatalogBrand> CatalogBrands { get; set; } = null!;

    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

    public DbSet<CatalogType> CatalogTypes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = GetConnectionString(_config);
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }

    private string GetConnectionString(DatabaseConfig config)
    {
        var envVarDb = config.EnvVar;

        var connectionString = envVarDb != null
            ? Environment.GetEnvironmentVariable(envVarDb) ?? config.ConnectionString
            : config.ConnectionString;

        if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
        {
            var uri = new Uri(connectionString);

            var host = uri.Host;
            var port = uri.Port;
            var database = uri.AbsolutePath.TrimStart('/');
            var userInfo = uri.UserInfo.Split(':', 2);
            var uid = userInfo[0];
            var password = userInfo[1];

            var keyValueConnectionString =
                $"server={host};port={port};database={database};uid={uid};password={password};sslmode=require;Trust Server Certificate=true;";

            var isEnvDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            if (isEnvDevelopment)
            {
                keyValueConnectionString += "Include Error Detail=true;";
            }

            return keyValueConnectionString;
        }
        else
        {
            return connectionString;
        }
    }
}

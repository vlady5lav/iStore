namespace Basket.Host.Configurations;

public class RedisConfig
{
    public const string Redis = "Redis";

    public TimeSpan CacheTimeout { get; set; }

    public string ConnectionString { get; set; } = null!;

    public string EnvVar { get; set; } = null!;
}

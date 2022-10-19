using Basket.Host.Configurations;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public class RedisCacheConnectionService : IRedisCacheConnectionService, IDisposable
{
    private readonly Lazy<ConnectionMultiplexer> _connectionLazy;

    private bool _disposed;

    public RedisCacheConnectionService(
        IOptionsMonitor<RedisConfig> config)
    {
        var envVar = config.CurrentValue.EnvVar;

        var connectionString = envVar != null
            ? Environment.GetEnvironmentVariable(envVar) ?? config.CurrentValue.ConnectionString
            : config.CurrentValue.ConnectionString;

        var keyValueConnectionString = GetRedisConnectionString(connectionString);

        var redisConfigurationOptions = ConfigurationOptions.Parse(keyValueConnectionString);
        redisConfigurationOptions.AllowAdmin = true;
        redisConfigurationOptions.AbortOnConnectFail = false;

        var isEnvDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        if (!isEnvDevelopment)
        {
            redisConfigurationOptions.CheckCertificateRevocation = true;
            redisConfigurationOptions.Ssl = true;
        }

        _connectionLazy =
            new Lazy<ConnectionMultiplexer>(()
                => ConnectionMultiplexer.Connect(redisConfigurationOptions));
    }

    public IConnectionMultiplexer Connection => _connectionLazy.Value;

    public void Dispose()
    {
        if (!_disposed)
        {
            Connection.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    private string GetRedisConnectionString(string connectionString)
    {
        if (connectionString.StartsWith("redis://") || connectionString.StartsWith("rediss://"))
        {
            var uri = new Uri(connectionString);

            var host = uri.Host;
            var port = uri.Port;
            var userInfo = uri.UserInfo.Split(':', 2);
            var user = userInfo[0];
            var password = userInfo[1];

            var keyValueConnectionString = $"{host}:{port},user={user},password={password}";

            if (connectionString.StartsWith("rediss://"))
            {
                keyValueConnectionString += ",ssl=true,checkCertificateRevocation=false";
            }

            return keyValueConnectionString;
        }
        else
        {
            return connectionString;
        }
    }
}

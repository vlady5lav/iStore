using Infrastructure.Configurations;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static void AddConfiguration(this WebApplicationBuilder builder, IConfiguration? configuration = null)
    {
        configuration ??= builder.Configuration;

        builder.Services.Configure<AppConfig>(
            configuration.GetSection(AppConfig.App));

        builder.Services.Configure<AuthorizationConfig>(
            configuration.GetSection(AuthorizationConfig.Authorization));

        builder.Services.Configure<ClientConfig>(
            configuration.GetSection(ClientConfig.Client));

        builder.Services.Configure<NginxConfig>(
            configuration.GetSection(NginxConfig.Nginx));
    }
}

using Infrastructure.Configurations;
using Infrastructure.Identity;

namespace Infrastructure.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorization(this WebApplicationBuilder builder)
    {
        var authConfig = builder.Configuration.GetSection(AuthorizationConfig.Authorization).Get<AuthorizationConfig>();

        var authority = authConfig.Authority;
        var siteAudience = authConfig.SiteAudience;

        var services = builder.Services;

        services
            .AddSingleton<IAuthorizationHandler, ScopeHandler>();

        services
            .AddAuthentication()
            .AddJwtBearer(AuthScheme.Internal, options =>
            {
                options.Authority = authority;
                options.Audience = siteAudience;

                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                };
            })
            .AddJwtBearer(AuthScheme.Site, options =>
            {
                options.Authority = authority;
                options.Audience = siteAudience;

                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                };
            });

        services
            .AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicy.AllowEndUserPolicy, policy =>
                {
                    policy.AuthenticationSchemes.Add(AuthScheme.Site);
                    policy.RequireClaim(JwtRegisteredClaimNames.Sub);
                });

                options.AddPolicy(AuthPolicy.AllowClientPolicy, policy =>
                {
                    policy.AuthenticationSchemes.Add(AuthScheme.Internal);
                    policy.Requirements.Add(new DenyAnonymousAuthorizationRequirement());
                    policy.Requirements.Add(new ScopeRequirement());
                });
            });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
}

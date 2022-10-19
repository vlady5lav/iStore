using Duende.IdentityServer.Models;

namespace IdentityServer.Configurations;

public static class AccessConfig
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("basket", "Basket"),
            new ApiScope("basket.bff", "Basket Bff"),
            new ApiScope("catalog", "Catalog"),
            new ApiScope("catalog.bff", "Catalog BFF"),
            new ApiScope("spa", "SPA"),
        };

    public static IEnumerable<Client> Clients(AppConfig configuration)
    {
        return new Client[]
        {
            new Client
            {
                ClientId = "spa_pkce",
                ClientName = "SPA",
                ClientSecrets = { new Secret("secret".Sha256()) },
                ClientUri = configuration.GlobalUrl,

                AllowedCorsOrigins = { configuration.GlobalUrl, configuration.IdentityUrl, configuration.SpaUrl },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = { "openid", "profile", "spa" },

                RedirectUris =
                {
                    $"{configuration.GlobalUrl}",
                    $"{configuration.GlobalUrl}/login/callback",
                    $"{configuration.GlobalUrl}/logout/callback",
                    $"{configuration.GlobalUrl}/signin/callback",
                    $"{configuration.GlobalUrl}/signin-callback-oidc",
                    $"{configuration.GlobalUrl}/signin-oidc",
                    $"{configuration.GlobalUrl}/signout/callback",
                    $"{configuration.GlobalUrl}/signout-callback-oidc",
                    $"{configuration.GlobalUrl}/signout-oidc",
                    $"{configuration.GlobalUrl}/silentrenew",

                    $"{configuration.SpaUrl}",
                    $"{configuration.SpaUrl}/login/callback",
                    $"{configuration.SpaUrl}/logout/callback",
                    $"{configuration.SpaUrl}/signin/callback",
                    $"{configuration.SpaUrl}/signin-callback-oidc",
                    $"{configuration.SpaUrl}/signin-oidc",
                    $"{configuration.SpaUrl}/signout/callback",
                    $"{configuration.SpaUrl}/signout-callback-oidc",
                    $"{configuration.SpaUrl}/signout-oidc",
                    $"{configuration.SpaUrl}/silentrenew",
                },

                PostLogoutRedirectUris =
                {
                    $"{configuration.GlobalUrl}",
                    $"{configuration.GlobalUrl}/logout/callback",
                    $"{configuration.GlobalUrl}/signout/callback",
                    $"{configuration.GlobalUrl}/signout-callback-oidc",
                    $"{configuration.GlobalUrl}/signout-oidc",

                    $"{configuration.SpaUrl}",
                    $"{configuration.SpaUrl}/logout/callback",
                    $"{configuration.SpaUrl}/signout/callback",
                    $"{configuration.SpaUrl}/signout-callback-oidc",
                    $"{configuration.SpaUrl}/signout-oidc",
                },

                AbsoluteRefreshTokenLifetime = 2592000,
                AccessTokenLifetime = 3600,
                AccessTokenType = AccessTokenType.Jwt,
                AllowAccessTokensViaBrowser = true,
                AllowOfflineAccess = true,
                AllowPlainTextPkce = false,
                AllowRememberConsent = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                AuthorizationCodeLifetime = 300,
                AlwaysSendClientClaims = true,
                BackChannelLogoutSessionRequired = true,
                ConsentLifetime = null,
                CoordinateLifetimeWithUserSession = true,
                Enabled = true,
                EnableLocalLogin = true,
                FrontChannelLogoutSessionRequired = true,
                IdentityTokenLifetime = 300,
                IncludeJwtId = true,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                RefreshTokenUsage = TokenUsage.ReUse,
                RequireClientSecret = true,
                RequireConsent = false,
                RequirePkce = true,
                RequireRequestObject = false,
                SlidingRefreshTokenLifetime = 1296000,
                UpdateAccessTokenClaimsOnRefresh = true,
            },

            new Client
            {
                ClientId = "basketswaggerui",
                ClientName = "Basket Swagger UI",
                ClientUri = configuration.BasketApi,

                AllowedCorsOrigins = { configuration.BasketApi, configuration.GlobalUrl, configuration.IdentityUrl },
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = { "basket", "basket.bff", "openid", "profile", "spa" },

                RedirectUris =
                {
                    $"{configuration.BasketApi}/basket/swagger/oauth2-redirect.html",
                    $"{configuration.BasketApi}/swagger/oauth2-redirect.html",
                    $"{configuration.GlobalUrl}/basket/swagger/oauth2-redirect.html",
                    $"{configuration.GlobalUrl}/swagger/oauth2-redirect.html",
                },

                PostLogoutRedirectUris =
                {
                    $"{configuration.BasketApi}/basket/swagger/",
                    $"{configuration.BasketApi}/swagger/",
                    $"{configuration.GlobalUrl}/basket/swagger/",
                    $"{configuration.GlobalUrl}/swagger/",
                },

                AllowAccessTokensViaBrowser = true,
            },

            new Client
            {
                ClientId = "catalogswaggerui",
                ClientName = "Catalog Swagger UI",
                ClientUri = configuration.CatalogApi,

                AllowedCorsOrigins = { configuration.CatalogApi, configuration.GlobalUrl, configuration.IdentityUrl },
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = { "catalog", "catalog.bff", "openid", "profile", "spa" },

                RedirectUris =
                {
                    $"{configuration.CatalogApi}/catalog/swagger/oauth2-redirect.html",
                    $"{configuration.CatalogApi}/swagger/oauth2-redirect.html",
                    $"{configuration.GlobalUrl}/catalog/swagger/oauth2-redirect.html",
                    $"{configuration.GlobalUrl}/swagger/oauth2-redirect.html",
                },

                PostLogoutRedirectUris =
                {
                    $"{configuration.CatalogApi}/catalog/swagger/",
                    $"{configuration.CatalogApi}/swagger/",
                    $"{configuration.GlobalUrl}/catalog/swagger/",
                    $"{configuration.GlobalUrl}/swagger/",
                },

                AllowAccessTokensViaBrowser = true,
            },
        };
    }
}

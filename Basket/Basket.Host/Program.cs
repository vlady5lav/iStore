using Basket.Host.Configurations;
using Basket.Host.Services;
using Basket.Host.Services.Interfaces;

var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

var webApplicationOptions = new WebApplicationOptions()
{
    ContentRootPath = baseDirectory,
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

builder.ConfigureServices();

builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection(RedisConfig.Redis));

var authConfig = builder.Configuration.GetSection(AuthorizationConfig.Authorization).Get<AuthorizationConfig>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShop - Basket HTTP API",
        Version = "v1",
        Description = "The Basket Service HTTP API",
    });

    var authority = authConfig.Authority;

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                TokenUrl = new Uri($"{authority}/connect/token"),
                Scopes = new Dictionary<string, string>()
                {
                    { "basket", "basket" },
                    { "basket.bff", "basket.bff" },
                },
            },
        },
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddTransient<IJsonSerializer, JsonSerializer>();
builder.Services.AddTransient<IRedisCacheConnectionService, RedisCacheConnectionService>();
builder.Services.AddTransient<ICacheService, CacheService>();
builder.Services.AddTransient<IBasketService, BasketService>();

var app = builder.Build();

app.ConfigurePipeline();

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.RoutePrefix = "swagger";
        setup.SwaggerEndpoint("v1/swagger.json", "Basket.API V1");
        setup.OAuthClientId("basketswaggerui");
        setup.OAuthAppName("Basket Swagger UI");
    });

app.Run();

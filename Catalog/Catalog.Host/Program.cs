using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;

var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

var webApplicationOptions = new WebApplicationOptions()
{
    ContentRootPath = baseDirectory,
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

builder.ConfigureServices();

builder.Services.Configure<CatalogConfig>(builder.Configuration.GetSection(CatalogConfig.Catalog));
builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(DatabaseConfig.Database));

var authConfig = builder.Configuration.GetSection(AuthorizationConfig.Authorization).Get<AuthorizationConfig>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShop - Catalog HTTP API",
        Version = "v1",
        Description = "The Catalog Service HTTP API",
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
                    { "catalog", "catalog" },
                    { "catalog.bff", "catalog.bff" },
                },
            },
        },
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<ICatalogRepository, CatalogRepository>();
builder.Services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
builder.Services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();

builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<ICatalogBrandService, CatalogBrandService>();
builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
builder.Services.AddTransient<ICatalogTypeService, CatalogTypeService>();

builder.Services.AddDbContextFactory<ApplicationDbContext>();

builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

var app = builder.Build();

app.ConfigurePipeline();

app.UseSwagger()
    .UseSwaggerUI(setup =>
    {
        setup.RoutePrefix = "swagger";
        setup.SwaggerEndpoint("v1/swagger.json", "Catalog.API V1");
        setup.OAuthClientId("catalogswaggerui");
        setup.OAuthAppName("Catalog Swagger UI");
    });

InitializeDB(app);

app.Run();

void InitializeDB(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            DbInitializer.Initialize(context, logger).Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred while creating the Database!\n" +
                $"[Ex: {ex.Message}]\n[InnerEx: {ex.InnerException?.Message}]");
        }
    }
}

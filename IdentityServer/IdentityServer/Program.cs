using IdentityServer.Extensions;

using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up...");

try
{
    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

    var webApplicationOptions = new WebApplicationOptions()
    {
        ContentRootPath = baseDirectory,
    };

    var builder = WebApplication.CreateBuilder(webApplicationOptions);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception!");
}
finally
{
    Log.Information("Shut down complete!");
    Log.CloseAndFlush();
}

using Infrastructure.Configurations;
using Infrastructure.Filters;

namespace Infrastructure.Extensions;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.Sources.Clear();

            var env = hostingContext.HostingEnvironment;

            config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);

            config.AddInMemoryCollection();

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                if (!string.IsNullOrEmpty(env.ApplicationName))
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true);
                    }
                }
            }

            config.AddEnvironmentVariables(prefix: "ASPNETCORE_");

            config.AddEnvironmentVariables();

            var args = Environment.GetCommandLineArgs();

            if (args != null)
            {
                config.AddCommandLine(args);
            }
        });

        builder.AddConfiguration();

        // 1st variant how to get desired configuration for WebApplicationBuilder in Program.cs
        //var appConfig = new AppConfig();
        //builder.Configuration.GetSection(AppConfig.App).Bind(appConfig);

        // 2nd variant how to get desired configuration for WebApplicationBuilder in Program.cs
        var appConfig = builder.Configuration.GetSection(AppConfig.App).Get<AppConfig>();
        var authConfig = builder.Configuration.GetSection(AuthorizationConfig.Authorization).Get<AuthorizationConfig>();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedHost |
                ForwardedHeaders.XForwardedProto;
            options.ForwardLimit = 2;
            options.RequireHeaderSymmetry = false;

            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        builder.Services.AddCertificateForwarding(options => { });

        builder.Services.AddHsts(options =>
        {
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(60);
            options.Preload = true;
        });

        builder.Services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;

            var httpsPort = builder.Configuration["HTTPS_PORT"] ?? Environment.GetEnvironmentVariable("HTTPS_PORT");

            var isPortParsed = int.TryParse(httpsPort, out var portParsed);

            if (isPortParsed)
            {
                options.HttpsPort = portParsed;
            }
        });

        builder.Services.AddCookiePolicy(options =>
        {
            options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.Secure = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = false;
            options.Cookie.Expiration = TimeSpan.FromDays(30);
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
        });

        builder.Services.ConfigureExternalCookie(options =>
        {
            options.Cookie.HttpOnly = false;
            options.Cookie.Expiration = TimeSpan.FromDays(30);
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
        });

        builder.Services.AddCors(
            options => options
                .AddPolicy(
                    "CorsPolicy",
                    corsBuilder => corsBuilder
                        //.SetIsOriginAllowed((host) => true)
                        .WithOrigins(
                            authConfig.Authority,
                            appConfig.BaseUrl,
                            appConfig.GlobalUrl,
                            appConfig.SpaUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()));

        builder.Services
            .AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)))
            .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

        builder.AddHttpLoggingConfiguration();
        builder.AddNginxConfiguration();
        builder.AddAuthorization();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // a variant how to get desired configuration for WebApplication in Program.cs
        var webAppConfig = app.Services.GetRequiredService<IOptionsMonitor<AppConfig>>().CurrentValue;

        var basePath = webAppConfig.BasePath;

        if (!string.IsNullOrEmpty(basePath))
        {
            app.UsePathBase(basePath);
        }

        if (webAppConfig.HttpLogging == "true")
        {
            app.UseHttpLogging();

            app.Use(async (ctx, next) =>
            {
                var remoteAddress = ctx.Connection.RemoteIpAddress;
                var remotePort = ctx.Connection.RemotePort;

                app.Logger.LogInformation($"Request Remote: {remoteAddress}:{remotePort}");

                await next(ctx);
            });
        }

        var forwardedHeadersOptions = new ForwardedHeadersOptions()
        {
            ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedHost |
                ForwardedHeaders.XForwardedProto,
            ForwardLimit = 2,
            RequireHeaderSymmetry = false,
        };

        forwardedHeadersOptions.KnownNetworks.Clear();
        forwardedHeadersOptions.KnownProxies.Clear();

        app.UseForwardedHeaders(forwardedHeadersOptions);

        app.UseCertificateForwarding();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");

            // The default HSTS value is 30 days.
            // see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        //app.UseDefaultFiles();
        //app.UseStaticFiles();

        var cookiePolicyOptions = new CookiePolicyOptions()
        {
            HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.None,
            MinimumSameSitePolicy = SameSiteMode.None,
            Secure = CookieSecurePolicy.SameAsRequest,
        };

        app.UseCookiePolicy(cookiePolicyOptions);

        app.UseRouting();

        //app.UseRequestLocalization();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        //app.UseSession();
        //app.UseResponseCompression();
        //app.UseResponseCaching();

        //app.MapControllers();
        //app.MapDefaultControllerRoute();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });

        return app;
    }

    public static WebApplicationBuilder AddNginxConfiguration(this WebApplicationBuilder builder)
    {
        var appConfig = builder.Configuration.GetSection(AppConfig.App).Get<AppConfig>();
        var nginxConfig = builder.Configuration.GetSection(NginxConfig.Nginx).Get<NginxConfig>();

        if (nginxConfig.UseNginx == "true")
        {
            try
            {
                if (nginxConfig.UseInitFile == "true")
                {
                    var initFile = nginxConfig.InitFilePath ?? "/tmp/app-initialized";

                    if (!File.Exists(initFile))
                    {
                        File.Create(initFile).Close();
                    }

                    File.SetLastWriteTimeUtc(initFile, DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Variable <UseNginx> is set to 'true', but there was an exception while configuring Initialize File:\n{ex.Message}");
            }

            try
            {
                if (nginxConfig.UseUnixSocket == "true")
                {
                    var unixSocket = nginxConfig.UnixSocketPath ?? "/tmp/nginx.socket";

                    builder.WebHost.ConfigureKestrel(kestrel =>
                    {
                        kestrel.ListenUnixSocket(unixSocket);
                        kestrel.AllowAlternateSchemes = true;
                    });
                }

                if (nginxConfig.UsePort == "true")
                {
                    var portParsed = int.TryParse(nginxConfig.Port, out var port);

                    if (portParsed)
                    {
                        builder.WebHost.ConfigureKestrel(kestrel =>
                        {
                            kestrel.ListenAnyIP(port);
                            kestrel.AllowAlternateSchemes = true;
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Variable <UseNginx> is set to 'true', but there was an exception while configuring Kestrel:\n{ex.Message}");
            }
        }
        else
        {
            var portEnv = builder.Configuration["PORT"] ?? Environment.GetEnvironmentVariable("PORT");

            try
            {
                if (portEnv != null)
                {
                    var portParsed = int.TryParse(portEnv, out var port);

                    if (portParsed)
                    {
                        builder.WebHost.ConfigureKestrel(kestrel =>
                        {
                            kestrel.ListenAnyIP(port);
                            kestrel.AllowAlternateSchemes = true;
                        });
                    }
                }
                else
                {
                    var baseUrl = appConfig.BaseUrl ?? appConfig.GlobalUrl;

                    if (baseUrl != null)
                    {
                        try
                        {
                            var basePort = new Uri(baseUrl)?.Port;

                            if (basePort is int @port)
                            {
                                builder.WebHost.ConfigureKestrel(kestrel =>
                                {
                                    kestrel.ListenAnyIP(@port);
                                    kestrel.AllowAlternateSchemes = true;
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"There was an exception while configuring Kestrel:\n{ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Variable <PORT> is set to '{portEnv}', but there was an exception while configuring Kestrel:\n{ex.Message}");
            }
        }

        return builder;
    }

    public static WebApplicationBuilder AddHttpLoggingConfiguration(this WebApplicationBuilder builder)
    {
        var appConfig = builder.Configuration.Get<AppConfig>();

        if (appConfig.HttpLogging == "true")
        {
            builder.Services.AddHttpLogging(options =>
                {
                    options.RequestHeaders.Add("Accept");
                    options.RequestHeaders.Add("Accept-Encoding");
                    options.RequestHeaders.Add("Accept-Language");
                    options.RequestHeaders.Add("Accept-Ranges");
                    options.RequestHeaders.Add("Access-Control-Allow-Headers");
                    options.RequestHeaders.Add("Access-Control-Allow-Methods");
                    options.RequestHeaders.Add("Access-Control-Allow-Origin");
                    options.RequestHeaders.Add("Access-Control-Request-Headers");
                    options.RequestHeaders.Add("Access-Control-Request-Method");
                    options.RequestHeaders.Add("Access-Control-Request-Origin");
                    options.RequestHeaders.Add("Authorization");
                    options.RequestHeaders.Add("Cache-Control");
                    options.RequestHeaders.Add("Connection");
                    options.RequestHeaders.Add("Connect-Time");
                    options.RequestHeaders.Add("Content-Length");
                    options.RequestHeaders.Add("Content-Security-Policy");
                    options.RequestHeaders.Add("Content-Type");
                    options.RequestHeaders.Add("Content-Type-Options");
                    options.RequestHeaders.Add("Cookie");
                    options.RequestHeaders.Add("Date");
                    options.RequestHeaders.Add("DNT");
                    options.RequestHeaders.Add("ETag");
                    options.RequestHeaders.Add("Forwarded-By");
                    options.RequestHeaders.Add("Forwarded-For");
                    options.RequestHeaders.Add("Forwarded-Host");
                    options.RequestHeaders.Add("Forwarded-Port");
                    options.RequestHeaders.Add("Forwarded-Proto");
                    options.RequestHeaders.Add("Frame-Options");
                    options.RequestHeaders.Add("Host");
                    options.RequestHeaders.Add("If-Modified-Since");
                    options.RequestHeaders.Add("If-None-Match");
                    options.RequestHeaders.Add("Last-Modified");
                    options.RequestHeaders.Add("Location");
                    options.RequestHeaders.Add("Method");
                    options.RequestHeaders.Add("Origin");
                    options.RequestHeaders.Add("Original-By");
                    options.RequestHeaders.Add("Original-For");
                    options.RequestHeaders.Add("Original-Host");
                    options.RequestHeaders.Add("Original-Port");
                    options.RequestHeaders.Add("Original-Proto");
                    options.RequestHeaders.Add("Path");
                    options.RequestHeaders.Add("PathBase");
                    options.RequestHeaders.Add("Pragma");
                    options.RequestHeaders.Add("Protocol");
                    options.RequestHeaders.Add("Real-IP");
                    options.RequestHeaders.Add("Referer");
                    options.RequestHeaders.Add("Referrer-Policy");
                    options.RequestHeaders.Add("Request-Id");
                    options.RequestHeaders.Add("Request-Start");
                    options.RequestHeaders.Add("Scheme");
                    options.RequestHeaders.Add("Sec-Ch-Ua");
                    options.RequestHeaders.Add("Sec-Ch-Ua-Mobile");
                    options.RequestHeaders.Add("Sec-Ch-Ua-Platform");
                    options.RequestHeaders.Add("Sec-Fetch-Dest");
                    options.RequestHeaders.Add("Sec-Fetch-Mode");
                    options.RequestHeaders.Add("Sec-Fetch-Site");
                    options.RequestHeaders.Add("Sec-Fetch-User");
                    options.RequestHeaders.Add("Sec-Gpc");
                    options.RequestHeaders.Add("Server");
                    options.RequestHeaders.Add("Set-Cookie");
                    options.RequestHeaders.Add("StatusCode");
                    options.RequestHeaders.Add("Strict-Transport-Security");
                    options.RequestHeaders.Add("Total-Route-Time");
                    options.RequestHeaders.Add("Transfer-Encoding");
                    options.RequestHeaders.Add("Upgrade");
                    options.RequestHeaders.Add("Upgrade-Insecure-Requests");
                    options.RequestHeaders.Add("User-Agent");
                    options.RequestHeaders.Add("Via");
                    options.RequestHeaders.Add("X-Content-Security-Policy");
                    options.RequestHeaders.Add("X-Content-Type-Options");
                    options.RequestHeaders.Add("X-Forwarded-By");
                    options.RequestHeaders.Add("X-Forwarded-For");
                    options.RequestHeaders.Add("X-Forwarded-Host");
                    options.RequestHeaders.Add("X-Forwarded-Port");
                    options.RequestHeaders.Add("X-Forwarded-Proto");
                    options.RequestHeaders.Add("X-Frame-Options");
                    options.RequestHeaders.Add("X-Original-By");
                    options.RequestHeaders.Add("X-Original-For");
                    options.RequestHeaders.Add("X-Original-Host");
                    options.RequestHeaders.Add("X-Original-Port");
                    options.RequestHeaders.Add("X-Original-Proto");
                    options.RequestHeaders.Add("X-Real-IP");
                    options.RequestHeaders.Add("X-Request-Id");
                    options.RequestHeaders.Add("X-Request-Start");

                    options.ResponseHeaders.Add("Accept");
                    options.ResponseHeaders.Add("Accept-Encoding");
                    options.ResponseHeaders.Add("Accept-Language");
                    options.ResponseHeaders.Add("Accept-Ranges");
                    options.ResponseHeaders.Add("Access-Control-Allow-Headers");
                    options.ResponseHeaders.Add("Access-Control-Allow-Methods");
                    options.ResponseHeaders.Add("Access-Control-Allow-Origin");
                    options.ResponseHeaders.Add("Access-Control-Request-Headers");
                    options.ResponseHeaders.Add("Access-Control-Request-Method");
                    options.ResponseHeaders.Add("Access-Control-Request-Origin");
                    options.ResponseHeaders.Add("Authorization");
                    options.ResponseHeaders.Add("Cache-Control");
                    options.ResponseHeaders.Add("Connection");
                    options.ResponseHeaders.Add("Connect-Time");
                    options.ResponseHeaders.Add("Content-Length");
                    options.ResponseHeaders.Add("Content-Security-Policy");
                    options.ResponseHeaders.Add("Content-Type");
                    options.ResponseHeaders.Add("Content-Type-Options");
                    options.ResponseHeaders.Add("Cookie");
                    options.ResponseHeaders.Add("Date");
                    options.ResponseHeaders.Add("DNT");
                    options.ResponseHeaders.Add("ETag");
                    options.ResponseHeaders.Add("Forwarded-By");
                    options.ResponseHeaders.Add("Forwarded-For");
                    options.ResponseHeaders.Add("Forwarded-Host");
                    options.ResponseHeaders.Add("Forwarded-Port");
                    options.ResponseHeaders.Add("Forwarded-Proto");
                    options.ResponseHeaders.Add("Frame-Options");
                    options.ResponseHeaders.Add("Host");
                    options.ResponseHeaders.Add("If-Modified-Since");
                    options.ResponseHeaders.Add("If-None-Match");
                    options.ResponseHeaders.Add("Last-Modified");
                    options.ResponseHeaders.Add("Location");
                    options.ResponseHeaders.Add("Method");
                    options.ResponseHeaders.Add("Origin");
                    options.ResponseHeaders.Add("Original-By");
                    options.ResponseHeaders.Add("Original-For");
                    options.ResponseHeaders.Add("Original-Host");
                    options.ResponseHeaders.Add("Original-Port");
                    options.ResponseHeaders.Add("Original-Proto");
                    options.ResponseHeaders.Add("Path");
                    options.ResponseHeaders.Add("PathBase");
                    options.ResponseHeaders.Add("Pragma");
                    options.ResponseHeaders.Add("Protocol");
                    options.ResponseHeaders.Add("Real-IP");
                    options.ResponseHeaders.Add("Referer");
                    options.ResponseHeaders.Add("Referrer-Policy");
                    options.ResponseHeaders.Add("Request-Id");
                    options.ResponseHeaders.Add("Request-Start");
                    options.ResponseHeaders.Add("Scheme");
                    options.ResponseHeaders.Add("Sec-Ch-Ua");
                    options.ResponseHeaders.Add("Sec-Ch-Ua-Mobile");
                    options.ResponseHeaders.Add("Sec-Ch-Ua-Platform");
                    options.ResponseHeaders.Add("Sec-Fetch-Dest");
                    options.ResponseHeaders.Add("Sec-Fetch-Mode");
                    options.ResponseHeaders.Add("Sec-Fetch-Site");
                    options.ResponseHeaders.Add("Sec-Fetch-User");
                    options.ResponseHeaders.Add("Sec-Gpc");
                    options.ResponseHeaders.Add("Server");
                    options.ResponseHeaders.Add("Set-Cookie");
                    options.ResponseHeaders.Add("StatusCode");
                    options.ResponseHeaders.Add("Strict-Transport-Security");
                    options.ResponseHeaders.Add("Total-Route-Time");
                    options.ResponseHeaders.Add("Transfer-Encoding");
                    options.ResponseHeaders.Add("Upgrade");
                    options.ResponseHeaders.Add("Upgrade-Insecure-Requests");
                    options.ResponseHeaders.Add("User-Agent");
                    options.ResponseHeaders.Add("Via");
                    options.ResponseHeaders.Add("X-Content-Security-Policy");
                    options.ResponseHeaders.Add("X-Content-Type-Options");
                    options.ResponseHeaders.Add("X-Forwarded-By");
                    options.ResponseHeaders.Add("X-Forwarded-For");
                    options.ResponseHeaders.Add("X-Forwarded-Host");
                    options.ResponseHeaders.Add("X-Forwarded-Port");
                    options.ResponseHeaders.Add("X-Forwarded-Proto");
                    options.ResponseHeaders.Add("X-Frame-Options");
                    options.ResponseHeaders.Add("X-Original-By");
                    options.ResponseHeaders.Add("X-Original-For");
                    options.ResponseHeaders.Add("X-Original-Host");
                    options.ResponseHeaders.Add("X-Original-Port");
                    options.ResponseHeaders.Add("X-Original-Proto");
                    options.ResponseHeaders.Add("X-Real-IP");
                    options.ResponseHeaders.Add("X-Request-Id");
                    options.ResponseHeaders.Add("X-Request-Start");

                    options.LoggingFields =
                        HttpLoggingFields.RequestScheme |
                        HttpLoggingFields.RequestPropertiesAndHeaders |
                        HttpLoggingFields.ResponsePropertiesAndHeaders;
                });
        }

        return builder;
    }
}

using System.Diagnostics;
using Serilog;
using Wati.Template.Api.Installers;
using Wati.Template.Api.Middlewares.Extensions;
using Wati.Template.Common.Constants;

namespace Wati.Template.Api;

public class Program
{
    private static readonly string[] DevEnvironments =
    {
        Common.Enums.EnvironmentName.LocalDev.ToString(),
        Common.Enums.EnvironmentName.Development.ToString()
    };

    public static async Task Main(string[] args)
    {
        try
        {
            var hostBuilder = CreateHostBuilder(args);
            await using var host = hostBuilder.Build();
            Configure(host);

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            if (Log.Logger.GetType().Name == "SilentLogger")
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .CreateLogger();
            }

            Log.Fatal(ex, "Template.Service terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    #region Private Methods

    private static WebApplicationBuilder CreateHostBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder
            .WebHost
            .CaptureStartupErrors(true)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var envName = hostingContext.HostingEnvironment.EnvironmentName;

                config
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{envName}.json", true, true);

                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithCorrelationIdHeader(InputRequest.CorrelationIdHeaderName)
                    .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment)
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name);
#if DEBUG
                // Used to filter out potentially bad data due debugging.
                loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
            });

        ConfigureServices(builder);

        return builder;
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(builder.Configuration);

        builder.Services.AddOptions();

        builder.Services.AddCustomHealthChecks();

        builder.Services.InstallServicesInAssembly(builder.Configuration, builder.Environment);
    }

    private static void Configure(WebApplication app)
    {
        var envName = app.Environment.EnvironmentName;
        app.Logger.LogInformation($"Running app under {envName} environment");

        if (DevEnvironments.Contains(envName))
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseSerilogRequestLogging();

        app.UseCustomException();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCorsPolicy();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseSwaggerWithUI();

        app.UseRouteEndpoints();

        app.UseCustomHealthChecks();

        //app.UseHangfireDashboard("/jobs");
    }

    #endregion
}
using System.Text;
using StackExchange.Redis.Extensions.Core.Configuration;
using Wati.Template.Api.Installers.Interfaces;
using Wati.Template.Common.Configurations;

namespace Wati.Template.Api.Installers;

public class ConfigurationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services
            .Configure<RedisConfiguration>(configuration.GetSection("ExternalSource:Redis"))
            .Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"))
            .PostConfigure<RedisConfiguration>(BuildRedisConfig)
            .PostConfigure<AuthenticationConfiguration>(BuildAuthenticationConfig);
    }

    #region Private Methods

    private static void BuildAuthenticationConfig(AuthenticationConfiguration option)
    {
        option ??= new AuthenticationConfiguration();
    }

    private static void BuildRedisConfig(RedisConfiguration option)
    {
        option ??= new RedisConfiguration();
        option.Hosts = new[] { GetDefaultHost(option.Hosts) };
        option.Password ??= DecodePassword(Environment.GetEnvironmentVariable("REDIS_SECRET"));
    }

    private static RedisHost GetDefaultHost(IEnumerable<RedisHost> hosts) =>
        hosts?.FirstOrDefault() ?? new RedisHost
        {
            Port = int.Parse(Environment.GetEnvironmentVariable("REDIS_PORT")),
            Host = Environment.GetEnvironmentVariable("REDIS_HOST") ?? string.Empty
        };

    private static string DecodePassword(string password)
    {
        if (string.IsNullOrEmpty(password)) { return default; }

        var decodedBytes = Convert.FromBase64String(password);
        return Encoding.UTF8.GetString(decodedBytes);
    }

    #endregion
}
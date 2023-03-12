using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using Wati.Template.Api.Installers.Interfaces;

namespace Wati.Template.Api.Installers;

public class RedisCacheInstaller :IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var redisConfig = services
            .BuildServiceProvider()
            .GetService<IOptions<RedisConfiguration>>()
            ?.Value;

        services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(options => new[] { redisConfig });
    }
}
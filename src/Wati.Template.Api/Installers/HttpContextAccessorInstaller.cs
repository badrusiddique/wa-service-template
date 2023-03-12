using Microsoft.AspNetCore.HttpOverrides;
using Wati.Template.Api.Installers.Interfaces;

namespace Wati.Template.Api.Installers;

public class HttpContextAccessorInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
    }
}
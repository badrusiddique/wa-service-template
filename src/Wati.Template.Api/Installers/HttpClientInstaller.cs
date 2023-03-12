using Wati.Template.Api.Installers.Interfaces;
using Wati.Template.Api.Middlewares.Extensions;

namespace Wati.Template.Api.Installers;

public class HttpClientInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.CreateHttpClients();
    }
}
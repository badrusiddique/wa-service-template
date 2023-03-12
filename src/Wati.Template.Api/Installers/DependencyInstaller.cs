using Wati.Template.Api.Installers.Interfaces;

namespace Wati.Template.Api.Installers;

public class DependencyInstaller :IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // register all orchestrator layer

        // register all service layer

        // register all repository layer

        // register all builders 

        // register all validations layer

        // register all data-contexts layer
    }
}
using Wati.Template.Api.Installers.Interfaces;

namespace Wati.Template.Api.Installers;

public static class InstallerExtension
{
    public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var installers = typeof(Program).Assembly.ExportedTypes
            .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList();

        installers.ForEach(installer => installer.InstallServices(services, configuration, environment));
    }
}
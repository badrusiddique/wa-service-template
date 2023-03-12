using System.Reflection;
using AutoMapper;
using Wati.Template.Api.Installers.Interfaces;
using Wati.Template.Repository.Mapper;

namespace Wati.Template.Api.Installers;

public class AutoMapperInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(DomainMapper)));
    }
}
using Microsoft.EntityFrameworkCore;
using Wati.Template.Api.Installers.Interfaces;
using Wati.Template.Data;

namespace Wati.Template.Api.Installers;

public class DatabaseInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var connection = configuration.GetConnectionString("sqlConnection");

        if (connection.Contains("in-memory")) { return; }

        services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connection));
    }
}
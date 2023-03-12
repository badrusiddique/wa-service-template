using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wati.Template.Data;

namespace Wati.Template.Service.IntegrationTests
{
    public class CustomWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly SqliteConnection _connection;
        private readonly string _envName = Common.Enums.EnvironmentName.Testing.ToString();

        public CustomWebAppFactory()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", _envName);
            _connection = new SqliteConnection(Constant.ConnectionString);
            _connection.Open();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment(_envName)
                .ConfigureAppConfiguration((host, app) =>
                {
                    app.AddEnvironmentVariables();
                    app.AddJsonFile("appsettings.json", optional: true);
                    app.AddJsonFile($"appsettings.{_envName}.json", optional: true);
                })
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

                    services.Remove(descriptor);

                    services
                        .AddEntityFrameworkSqlite()
                        .AddDbContext<DatabaseContext>(options =>
                        {
                            options.UseSqlite(_connection);
                            options.UseInternalServiceProvider(services.BuildServiceProvider());
                        });

                    InitializeDBForTests(services);
                });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection.Close();
        }

        private void InitializeDBForTests(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var providers = scope.ServiceProvider;
            var dbContext = providers.GetRequiredService<DatabaseContext>();

            try
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"an error occurred initializing the database: {ex.Message}");
            }
        }
    }
}

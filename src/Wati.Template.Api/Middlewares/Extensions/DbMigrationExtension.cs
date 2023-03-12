using Microsoft.EntityFrameworkCore;
using Wati.Template.Data;

namespace Wati.Template.Api.Middlewares.Extensions;

public static class DbMigrateExtension
{
    public static void UseUpdateDatabase(this IApplicationBuilder app, bool isAutoMigrationEnabled)
    {
        if (isAutoMigrationEnabled)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Updating database using ef-auto-migration for DatabaseContext");
                context.Database.Migrate();
            }
            else
            {
                logger.LogInformation("Skipping ef-auto-migration for DatabaseContext as there are no pending migrations to update");
            }

            // context.SeedDatabase().GetAwaiter().GetResult();
        }
    }
}
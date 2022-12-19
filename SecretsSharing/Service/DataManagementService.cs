using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SecretsSharing.Data;
using System;
using System.Threading;

namespace SecretsSharing.Service
{
    public static class DataManagementService
    {
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var db = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database;

                logger.LogInformation("Migrating database...");

                while (!db.CanConnect())
                {
                    logger.LogInformation("Database not ready yet; waiting...");
                    Thread.Sleep(1000);
                }

                try
                {
                    serviceScope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
                    logger.LogInformation("Database migrated successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
        }
    }
}

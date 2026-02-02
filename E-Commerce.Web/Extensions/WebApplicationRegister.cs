using ECommerce.Domain.Contracts;
using ECommerce.Persistence.Data.DbContexts;
using ECommerce.Persistence.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegister
    {
        #region Pending Migration

        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            return app;
        }
        public static async Task<WebApplication> MigrateIdentityDataBaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreIdentityDbContext>();

            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
            {
                dbContext.Database.Migrate();
            }

            return app;
        }

        #endregion



        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dataInitializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Default");

            await dataInitializer.InitializeAsync();

            return app;
        }

        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dataInitializer = scope.ServiceProvider.GetRequiredKeyedService<IDataInitializer>("Identity");

            await dataInitializer.InitializeAsync();

            return app;
        }
    }
}

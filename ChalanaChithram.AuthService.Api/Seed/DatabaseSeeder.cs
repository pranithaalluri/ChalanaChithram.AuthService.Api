//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using ChalanaChithram.AuthService.Api.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace ChalanaChithram.AuthService.Api.Seed;

//public static class DatabaseSeeder
//{
//    public static async Task SeedAsync(IServiceProvider services)
//    {
//        IServiceScope scope = services.CreateScope();
//        IServiceProvider provider = scope.ServiceProvider;

//        AppDbContext dbContext = provider.GetRequiredService<AppDbContext>();
//        ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
//        ILogger logger = loggerFactory.CreateLogger("AuthService.DatabaseSeeder");

//        try
//        {
//            await dbContext.Database.MigrateAsync();

//            if (!dbContext.AppUsers.Any())
//            {
//                dbContext.AppUsers.Add(AuthSeedData.GetDefaultUser());
//                await dbContext.SaveChangesAsync();
//            }

//            logger.LogInformation("AuthService DB migrated and seeded successfully.");
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "AuthService DB seeding failed.");
//            throw;
//        }
//        finally
//        {
//            scope.Dispose();
//        }
//    }
//}

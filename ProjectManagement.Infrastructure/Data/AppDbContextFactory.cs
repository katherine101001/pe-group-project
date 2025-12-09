using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectManagement.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                // .SetBasePath(Directory.GetCurrentDirectory()) // Make sure path points to Infrastructure project
                // .AddJsonFile("appsettings.json")
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ProjectManagement.API"))
                .AddJsonFile("appsettings.json")

                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("ProjectManagementDBConnectionString");

            // Configure DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AlcoPlus.Data;

public class AlcoPlusDbContextFactory : IDesignTimeDbContextFactory<AlcoPlusDbContext>
{
    public AlcoPlusDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AlcoPlusDbContext>();
        var connectionString = configuration.GetConnectionString("AlcoPlusApiDbConnectionString");

        optionsBuilder.UseSqlServer(connectionString);

        return new AlcoPlusDbContext(optionsBuilder.Options);
    }
}

using AlcoPlus.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlcoPlus.API.Data;

public class AlcoPlusDbContext : DbContext
{
    public AlcoPlusDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Hotel> Hotels { get; set; }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Philippines",
                    ShortName = "PH",
                },
                new Country
                {
                    Id = 2,
                    Name = "Japan",
                    ShortName = "JP",
                },
                new Country
                {
                    Id = 3,
                    Name = "Bahamas",
                    ShortName = "BH",
                }
            );

        modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Hotel 1",
                    Address = "Hotel address 1",
                    CountryId = 1,
                    Rating = 4.4
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Hotel 2",
                    Address = "Hotel address 2",
                    CountryId = 1,
                    Rating = 3.5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Hotel 3",
                    Address = "Hotel address 3",
                    CountryId = 2,
                    Rating = 4
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Hotel 4",
                    Address = "Hotel address 4",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 5,
                    Name = "Hotel 5",
                    Address = "Hotel address 5",
                    CountryId = 3,
                    Rating = 2.5
                }
            );
    }
}

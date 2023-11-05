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
}

using Microsoft.EntityFrameworkCore;

namespace AlcoPlus.API.Data;

public class AlcoPlusDbContext : DbContext
{
    public AlcoPlusDbContext(DbContextOptions options) : base(options)
    {
        
    }
}

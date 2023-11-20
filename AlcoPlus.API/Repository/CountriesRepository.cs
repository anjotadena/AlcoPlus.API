using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AlcoPlus.API.Repository;

public class CountriesRepository : Repository<Country>, ICountriesRepository
{
    private readonly AlcoPlusDbContext _context;

    public CountriesRepository(AlcoPlusDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
    }

    public async Task<Country> GetDetails(int id)
    {
        var country = await _context.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(q => q.Id == id);

        return country;
    }
}

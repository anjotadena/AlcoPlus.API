using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Entities;

namespace AlcoPlus.API.Repository;

public class CountriesRepository : Repository<Country>, ICountriesRepository
{
    public CountriesRepository(AlcoPlusDbContext context) : base(context)
    {
    }
}

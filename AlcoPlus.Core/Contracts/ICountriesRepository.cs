using AlcoPlus.Data;

namespace AlcoPlus.Core.Contracts;

public interface ICountriesRepository : IRepository<Country>
{
    Task<Country> GetDetails(int id);
}

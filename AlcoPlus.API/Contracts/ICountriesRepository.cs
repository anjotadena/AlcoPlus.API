﻿using AlcoPlus.API.Entities;

namespace AlcoPlus.API.Contracts;

public interface ICountriesRepository : IRepository<Country>
{
    Task<Country> GetDetails(int id);
}
using AutoMapper;
﻿using AlcoPlus.Core.Contracts;
using AlcoPlus.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AlcoPlus.Core.Models.Countries;
using AlcoPlus.Core.Exceptions;

namespace AlcoPlus.Core.Repository;

public class CountriesRepository : Repository<Country>, ICountriesRepository
{
    private readonly AlcoPlusDbContext _context;
    private readonly IMapper _mapper;

    public CountriesRepository(AlcoPlusDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Country> GetDetails(int id)
    {
        var country = await _context.Countries
                                    .Include(q => q.Hotels)
                                    .ProjectTo<Country>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync(q => q.Id == id);

        if (country is null)
        {
            throw new NotFoundException(nameof(GetDetails), id);
        }

        return country;
    }
}

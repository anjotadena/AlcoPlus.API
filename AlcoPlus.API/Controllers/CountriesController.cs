using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlcoPlus.API.Entities;
using AutoMapper;
using AlcoPlus.API.Models.Countries;
using AlcoPlus.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using AlcoPlus.API.Exceptions;
<<<<<<< Updated upstream
=======
using AlcoPlus.API.Models;
using Microsoft.AspNetCore.OData.Query;
>>>>>>> Stashed changes

namespace AlcoPlus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CountriesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICountriesRepository _countriesRepository;

    public CountriesController(IMapper mapper, ICountriesRepository countriesRepository)
    {
        _mapper = mapper;
        _countriesRepository = countriesRepository;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();

        return Ok(_mapper.Map<List<GetCountryDto>>(countries));
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        if (country is null)
        {
            throw new NotFoundException(nameof(GetCountry), id);
        }

        return Ok(_mapper.Map<CountryDto>(country));
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<GetCountryDto>> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
        {
            return BadRequest();
        }

        var country = await _countriesRepository.GetAsync(id);

        if (country is null)
        {
            throw new NotFoundException(nameof(PutCountry), id);
        }

        _mapper.Map(updateCountryDto, country);

        try
        {
            await _countriesRepository.UpdateAsync(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExists(id))
            {
                throw new NotFoundException(nameof(PutCountry), id);
            }

            throw;
        }

        return Ok(_mapper.Map<GetCountryDto>(country));
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto countryDto)
    {
        var country = _mapper.Map<Country>(countryDto);

        var result = await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", result);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);

        if (country is null)
        {
            throw new NotFoundException(nameof(DeleteCountry), id);
        }

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.Exists(id);
    }
}

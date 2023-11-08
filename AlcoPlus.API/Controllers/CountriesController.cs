using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlcoPlus.API.Entities;
using AutoMapper;
using AlcoPlus.API.Models.Countries;
using AlcoPlus.API.Contracts;

namespace AlcoPlus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<ActionResult<GetCountryDetailsDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        if (country is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<GetCountryDetailsDto>(country));
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<GetCountryDto>> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
        {
            return BadRequest();
        }

        var country = await _countriesRepository.GetAsync(id);

        if (country is null)
        {
            return NotFound();
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
                return NotFound();
            }

            throw;
        }

        return Ok(_mapper.Map<GetCountryDto>(country));
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
        var country = _mapper.Map<Country>(countryDto);

        var result = await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", result);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);

        if (country == null)
        {
            return NotFound();
        }

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.Exists(id);
    }
}

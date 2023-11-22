using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AlcoPlus.Core.Models.Countries;
using AlcoPlus.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using AlcoPlus.Core.Exceptions;
using Microsoft.AspNetCore.OData.Query;

namespace AlcoPlus.API.Controllers.v2;

[Route("api/v{version:apiVersion}/countries")]
[Asp.Versioning.ApiVersion("2.0")]
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
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync<GetCountryDto>();

        return Ok(countries);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        return Ok(country);
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

        GetCountryDto country;

        try
        {
            country = await _countriesRepository.UpdateAsync<UpdateCountryDto, GetCountryDto>(id, updateCountryDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CountryExists(id))
            {
                throw new NotFoundException(nameof(PutCountry), id);
            }

            throw;
        }

        return Ok(country);
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto countryDto)
    {
        var result = await _countriesRepository.AddAsync<CreateCountryDto, CountryDto>(countryDto);

        return CreatedAtAction(nameof(GetCountry), result.Id);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.Exists(id);
    }
}

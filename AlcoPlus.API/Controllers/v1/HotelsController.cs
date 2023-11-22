using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AlcoPlus.Core.Contracts;
using AlcoPlus.Core.Models.Hotels;
using AlcoPlus.Data;

namespace AlcoPlus.API.Controllers.v1;

[Route("api/v{version:apiVersion}/hotels")]
[ApiController]
[Asp.Versioning.ApiVersion("1.0")]
public class HotelsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IHotelsRepository _hotelsRepository;

    public HotelsController(IMapper mapper, IHotelsRepository hotelsRepository)
    {
        _mapper = mapper;
        _hotelsRepository = hotelsRepository;
    }

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<List<HotelDto>>> GetHotels()
    {
        var hotels = await _hotelsRepository.GetAllAsync<HotelDto>();

        return Ok(hotels);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync<HotelDto>(id);

        return Ok(hotel);
    }

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<HotelDto>> PutHotel(int id, UpdateHotelDto hotelDto)
    {
        if (id != hotelDto.Id)
        {
            return BadRequest();
        }

        HotelDto hotel;
        try
        {
            hotel = await _hotelsRepository.UpdateAsync<UpdateHotelDto, HotelDto>(id, hotelDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await HotelExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return Ok(hotel);
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
    {
        var result = await _hotelsRepository.AddAsync<CreateHotelDto, Hotel>(hotelDto);

        return CreatedAtAction("GetHotel", result);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await _hotelsRepository.Exists(id);
    }
}

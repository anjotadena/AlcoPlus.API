using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlcoPlus.API.Entities;
using AutoMapper;
using AlcoPlus.API.Contracts;
using AlcoPlus.API.Models.Hotels;
using Microsoft.AspNetCore.Authorization;

namespace AlcoPlus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        var hotels = await _hotelsRepository.GetAllAsync();

        _mapper.Map<List<HotelDto>>(hotels);

        return Ok(hotels);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<HotelDto>(hotel));
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
        
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel is null)
        {
            return NotFound();
        }

        _mapper.Map(hotel, hotelDto);

        try
        {
            await _hotelsRepository.UpdateAsync(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await HotelExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return Ok(_mapper.Map<HotelDto>(hotel));
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
    {
        var hotel = _mapper.Map<Hotel>(hotelDto);

        var result = await _hotelsRepository.AddAsync(hotel);

        return CreatedAtAction("GetHotel", result);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel is null)
        {
            return NotFound();
        }

        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExists(int id)
    {
        return await _hotelsRepository.Exists(id);
    }
}

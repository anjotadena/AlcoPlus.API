using AlcoPlus.API.Models.Hotels;

namespace AlcoPlus.API.Models.Countries;

public class CountryDto : BaseCountryDto
{
    public int Id { get; set; }

    public List<HotelDto> Hotels { get; set; }
}

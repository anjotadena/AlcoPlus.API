using AlcoPlus.Core.Models.Hotels;

namespace AlcoPlus.Core.Models.Countries;

public class CountryDto : BaseCountryDto
{
    public int Id { get; set; }

    public List<HotelDto> Hotels { get; set; }
}

using AlcoPlus.API.Models.Countries;

namespace AlcoPlus.API.Models.Hotels;

public class GetHotelDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public double Rating { get; set; }
}
